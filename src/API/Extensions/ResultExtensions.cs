using Application.Pattern;

namespace API.Extensions;

public static class ResultExtensions
{
    public static ProblemDetails ToProblemDetails(this Result result, string path, string title = "An error occurred")
    {
        return new ProblemDetails
        {
            Title = title,
            Detail = result.ErrorMessage,
            Status = StatusCodes.Status400BadRequest,
            Instance = path
        };
    }
}