using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace MidjourneyAPI.Utils;

public class DiscordCaller:IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DiscordCaller> _log;
    private readonly string _interactionsUrl = "https://discord.com/api/v9/interactions";
    private readonly string _retrieveMessageUrl = "https://discord.com/api/v10/channels/CHANNELID/messages?limit=LIMIT";

    public DiscordCaller(ILogger<DiscordCaller> log)
    {
        _log = log;
        _httpClient = new HttpClient();
    }

    public async Task<int> InteractionsAsync<T>(DiscordRequest<T> request, DiscordOption option) where T: class
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, _interactionsUrl);
        requestMessage.Headers.TryAddWithoutValidation("Authorization", option.DiscordToken);
        var content = new StringContent(JsonConvert.SerializeObject(request));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        requestMessage.Content = content;

        var response = await _httpClient.SendAsync(requestMessage);
        // for discord api rate limit
        await Task.Delay(950);
        if ((int)response.StatusCode >= 400)
        {
            _log.LogWarning($"Error config: {JsonConvert.SerializeObject(option)}");
        }

        return (int)response.StatusCode;
    }

    public async Task<List<DiscordMessageDto>?> RetrieveMessageAsync( DiscordOption option,int limit = 50)
    {
        var url = _retrieveMessageUrl.Replace("CHANNELID", option.ChannelId).Replace("LIMIT", limit.ToString());
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
        requestMessage.Headers.TryAddWithoutValidation("Authorization", option.DiscordToken);
        
        var response = await _httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
        {
            _log.LogError($"Error config: {JsonConvert.SerializeObject(option)}");
            _log.LogError($"Http error! status: {response.StatusCode}");
            return null;
        }

        var strData = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(strData))
        {
            return null;
        }

        return JsonConvert.DeserializeObject<List<DiscordMessageDto>>(strData);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}