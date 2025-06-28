namespace Application.Models;

public abstract record BaseResponse
{
    [JsonPropertyOrder(-4)]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyOrder(-3)]
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyOrder(-2)]
    [JsonPropertyName("modifiedAt")]
    public DateTime? ModifiedAt { get; set; }

    [JsonPropertyOrder(-1)]
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
}