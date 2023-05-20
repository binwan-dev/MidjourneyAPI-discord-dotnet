namespace MidjourneyAPI.Options;

public class DiscordOption
{
    private static DiscordOption _defaultOptions = new();
    
    public string DiscordToken { get; set; } = string.Empty;
    
    public string ServerId { get; set; } = string.Empty;

    public string ChannelId { get; set; } = string.Empty;

    public void SetOption(DiscordOption newOption)
    {
        if (_defaultOptions.DiscordToken != newOption.DiscordToken)
        {
            DiscordToken = newOption.DiscordToken;
        }

        if (_defaultOptions.ServerId != newOption.ServerId)
        {
            ServerId = newOption.ServerId;
        }

        if (_defaultOptions.ChannelId != newOption.ChannelId)
        {
            ChannelId = newOption.ChannelId;
        }
    }

    public virtual void CheckConfig()
    {
        if(string.IsNullOrWhiteSpace(DiscordToken))
        {
            throw new ArgumentNullException(nameof(DiscordToken));
        }

        if(string.IsNullOrWhiteSpace(ServerId))
        {
            throw new ArgumentNullException(nameof(DiscordToken));
        }
        
        if(string.IsNullOrWhiteSpace(ChannelId))
        {
            throw new ArgumentNullException(nameof(DiscordToken));
        }
    }
}