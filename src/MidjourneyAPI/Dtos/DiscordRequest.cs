namespace MidjourneyAPI.Dtos;

public record DiscordRequest<T> where T: class
{
    [JsonProperty("type")]
    public int Type { get; set; } = 2;

    [JsonProperty("application_id")] 
    public string ApplicationId { get; set; } = "936929561302675456";
    
    [JsonProperty("guild_id")] 
    public string GuildId { get; set; } = string.Empty;

    [JsonProperty("channel_id")] 
    public string ChannelId { get; set; } = string.Empty;
    
    [JsonProperty("session_id")] 
    public string SessionId { get; set; } = string.Empty;

    [JsonProperty("data")] 
    public T Data { get; set; } = null!;
}