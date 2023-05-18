namespace MidjourneyAPI.Options;

public class DiscordOption
{
    private static DiscordOption _defaultOptions = new();
    
    public string SalaiToken { get; set; } = string.Empty;
    
    public string ServerId { get; set; } = string.Empty;

    public string ChannelId { get; set; } = string.Empty;

    public void SetOption(DiscordOption newOption)
    {
        if (_defaultOptions.SalaiToken != newOption.SalaiToken)
        {
            SalaiToken = newOption.SalaiToken;
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
}