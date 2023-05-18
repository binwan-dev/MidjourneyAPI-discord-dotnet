namespace MidjourneyAPI.Dtos;

public record DiscordMessageDto
{
    [JsonProperty("id")] 
    public string Id { get; set; } = string.Empty;
    
    [JsonProperty("author")]
    public DiscordAuthorDto Author { get; set; } = null!;

    [JsonProperty("content")]
    public string Content { get; set; } = string.Empty;

    [JsonProperty("attachments")] 
    public List<DiscordAttachmentDto> Attachments { get; set; } = new();

    [JsonProperty("components")]
    public List<object> Components { get; set; } = new();
}

public record DiscordAuthorDto
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;
}

public record DiscordAttachmentDto
{
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    [JsonProperty("filename")]
    public string FileName { get; set; } = string.Empty;
}