using System.Text.Json.Serialization;

namespace Shared.Models;

public abstract record BaseResponse(
    [property: JsonPropertyOrder(-4)] Guid Id,
    [property: JsonPropertyOrder(-3)] DateTime CreatedAt,
    [property: JsonPropertyOrder(-2)] DateTime? ModifiedAt,
    [property: JsonPropertyOrder(-1)] bool IsActive
    );