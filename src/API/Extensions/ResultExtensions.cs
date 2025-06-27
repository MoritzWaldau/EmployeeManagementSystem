using System.Net;
using Application.Pattern;

namespace API.Extensions;

public static class ResultExtensions
{
    
    public static ProblemDetails HandleProblem(this Result result, string path, HttpStatusCode code, string title = "")
    {
        return code switch 
        {
            HttpStatusCode.BadRequest => new ProblemDetails
            {
                Title = string.IsNullOrEmpty(title) ? "Bad Request" : title,
                Detail = result.ErrorMessage,
                Status = StatusCodes.Status400BadRequest,
                Instance = path
            },
            HttpStatusCode.NotFound => new ProblemDetails
            {
                Title = string.IsNullOrEmpty(title) ? "Not Found" : title,
                Detail = result.ErrorMessage,
                Status = StatusCodes.Status404NotFound,
                Instance = path
            },
            _ => new ProblemDetails
            {
                Title = title,
                Detail = result.ErrorMessage,
                Status = StatusCodes.Status500InternalServerError,
                Instance = path
            }
        };
    }
    public static ProblemDetails ToProblemDetails<T>(this Result<T> result, string path, string title = "An error occurred")
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