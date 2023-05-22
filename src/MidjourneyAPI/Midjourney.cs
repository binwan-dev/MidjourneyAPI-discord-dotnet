using System.Diagnostics;
using System.Text.RegularExpressions;
using MidjourneyAPI.Utils;

namespace MidjourneyAPI;

public class Midjourney
{
    private readonly ILogger<Midjourney> _log;
    private readonly DiscordCaller _discordCaller;
    private readonly IOptions<MidjourneyOption> _midjourneyOption;

    public Midjourney(ILogger<Midjourney> log,DiscordCaller discordCaller,IOptions<MidjourneyOption> midjourneyOption)
    {
        _log = log;
        _discordCaller = discordCaller;
        _midjourneyOption = midjourneyOption;
    }

    public async Task<MidjourneyMessageDto?> ImagineAsync(string prompt, MidjourneyOption? customOption = null,
        Func<MidjourneyMessageDto, Task>? loading = null)
    {
        var option = _midjourneyOption.Value.Clone();
        if (customOption != null)
        {
            option.SetOption(customOption);
        }

        if (!prompt.Contains("--seed"))
        {
            prompt += $" --seed {new Random().Next(1000, 9999)}";
        }

        _log.LogInformation($"imagine -> {prompt}");
        var request = new DiscordRequest<MidjourneyImagineRequest>();
        request.Data = new MidjourneyImagineRequest();
        request.Data.Options.First().Value = prompt;
        request.GuildId = option.ServerId;
        request.ChannelId = option.ChannelId;
        request.SessionId = option.DiscordToken;
        var responseStatusCode = await _discordCaller.InteractionsAsync(request, option);
        if (responseStatusCode != 204)
        {
            throw new HttpRequestException($"Imagine api failed with status {responseStatusCode}");
        }

        _log.LogInformation("Await generate image");
        var msg = await WaitMessageAsync(prompt, option, loading);
        _log.LogInformation($"Image generated! prompt: {prompt}, url: {msg?.Url}");
        return msg;
    }

    private async Task<MidjourneyMessageDto?> WaitMessageAsync(string prompt, MidjourneyOption configOption, Func<MidjourneyMessageDto,Task>? loading=null)
    {
        var regex= @"/(<)?(https?:\/\/[^\s]*)(>)?/gi";
        prompt = prompt.Replace(regex, "").Trim();

        for (var i = 0; i < configOption.MaxWait; i++)
        {
            var msgs = await _discordCaller.RetrieveMessageAsync(configOption, configOption.Limit);
            if (msgs == null)
            {
                await NoMsgHandleAsync();
                continue;
            }

            var mjMsg = await FilterDiscordMessageAsync(prompt, msgs, loading: loading);
            if (mjMsg == null)
            {
                await NoMsgHandleAsync();
                continue;
            }

            return mjMsg;
        }
        
        return null;

        Task NoMsgHandleAsync()
        {
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug("Wait no message found!");
            }

            return Task.Delay(1000);
        }
    }

    private async Task<MidjourneyMessageDto?> FilterDiscordMessageAsync(string prompt,List<DiscordMessageDto> discordMessages,string? options=null, string index="", Func<MidjourneyMessageDto,Task>? loading=null)
    {
        var noUrlPrompt = prompt;
        if (prompt.StartsWith("http://") || prompt.StartsWith("https://"))
        {
            var url = prompt.Split(' ')[0];
            noUrlPrompt = prompt.Replace(url, "");
        }
        
        foreach (var msg in discordMessages)
        {
            
            if (msg.Author.Id != "936929561302675456" || !msg.Content.Contains(noUrlPrompt))
            {
                continue;
            }

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug($"Msg: {JsonConvert.SerializeObject(msg)}");
            }

            if (options != null && (msg.Content.Contains(options) ||
                                    (options == "Upscaled" && msg.Content.Contains($"Image #{index}"))))
            {
                _log.LogDebug("no option");
                continue;
            }

            if (msg.Attachments.Count == 0)
            {
                _log.LogDebug("no attachment");
                break;
            }

            // waiting
            var attachment = msg.Attachments.First();
            if (attachment.FileName.StartsWith("grid") || msg.Components.Count == 0)
            {
                _log.LogDebug($"content: {msg.Content}");
                var aRegex = @"(?<=\()(.+?)(?=\))";
                var progress = "wait";
                var match = Regex.Match(msg.Content, aRegex);
                if (match.Success)
                {
                    progress = match.Value;
                    if (loading != null)
                    {
                        await loading?.Invoke(new MidjourneyMessageDto(msg.Id, string.Empty, string.Empty, string.Empty, progress));
                    }
                }

                _log.LogDebug("No match found");
                break;
            }

            // finished
            var content = Regex.Split(msg.Content, "\\*\\*")[1];
            return new MidjourneyMessageDto(msg.Id, attachment.Url, attachment.Url, content, "done");
        }

        return null;
    }
}

public record MidjourneyImagineRequest
{
    [JsonProperty("version")] 
    public string Version { get; set; } = "1077969938624553050";
    
    [JsonProperty("id")] 
    public string Id { get; set; } = "938956540159881230";

    [JsonProperty("name")] 
    public string Name { get; set; } = "imagine";
    
    [JsonProperty("type")] 
    public int Type { get; set; } = 1;

    [JsonProperty("options")] 
    public List<MidjourneyImagineRequestOption> Options { get; set; } = new() {new()};

    [JsonProperty("attachments")]
    public List<string> Attachments { get; set; } = new();

    [JsonProperty("application_command")] 
    public MidjourneyApplicationCommand ApplicationCommand { get; set; } = new();
}

public record MidjourneyApplicationCommand
{
    [JsonProperty("id")]
    public string Id { get; set; } = "938956540159881230";
    
    [JsonProperty("application_id")]
    public string ApplicationId { get; set; } = "936929561302675456";
        
    [JsonProperty("version")]
    public string Version { get; set; } = "1077969938624553050";
    
    [JsonProperty("default_permission")]
    public bool DefaultPermission { get; set; } = true;

    [JsonProperty("default_member_permissions")]
    public List<string>? DefaultMemberPermissions { get; set; } = default;
        
    [JsonProperty("type")]
    public int Type { get; set; } = 1;

    [JsonProperty("nsfw")] 
    public bool Nsfw { get; set; } = false;
        
    [JsonProperty("name")]
    public string Name { get; set; } = "imagine";
        
    [JsonProperty("description")]
    public string Description { get; set; } = "Create images with Midjourney";
        
    [JsonProperty("dm_permission")]
    public bool DmPermission { get; set; } = true;
        
    [JsonProperty("options")]
    public List<MidjourneyApplicationCommandOption> Options { get; set; } = new (){new()};
}

public record MidjourneyImagineRequestOption
{
    [JsonProperty("type")]
    public int Type { get; set; } = 3;
    
    [JsonProperty("name")]
    public string Name { get; set; } = "prompt";
    
    [JsonProperty("value")]
    public string Value { get; set; } = "prompt";
}

public record MidjourneyApplicationCommandOption
{
    [JsonProperty("type")]
    public int Type { get; set; } = 3;

    [JsonProperty("name")] 
    public string Name { get; set; } = "prompt";
    
    [JsonProperty("description")]
    public string Description { get; set; } = "The prompt to imagine";
    
    [JsonProperty("required")]
    public bool Required { get; set; } = true;
}