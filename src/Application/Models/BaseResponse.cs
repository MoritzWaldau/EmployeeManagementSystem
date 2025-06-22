namespace Application.Models;


public abstract record BaseResponse
{
    [JsonPropertyOrder(-4)]
    public Guid Id { get; set; }

    [JsonPropertyOrder(-3)]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyOrder(-2)]
    public DateTime? ModifiedAt { get; set; }

    [JsonPropertyOrder(-1)]
    public bool IsActive { get; set; }
}