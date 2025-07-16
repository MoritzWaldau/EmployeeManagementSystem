namespace API.Extensions;

public static class ResultExtensions
{
    public static ProblemDetails ToProblemDetails(this Result result, string path, string title = "An error occurred")
    {
        if (result.ValidationErrors.Count == 0)
        {
            return new ProblemDetails
            {
                Title = title,
                Detail = result.ErrorMessage,
                Status = StatusCodes.Status400BadRequest,
                Instance = path
            };
        }
        
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Status = StatusCodes.Status400BadRequest,
            Detail = "Failed validation",
            Instance = path
        };
            
        problemDetails.Extensions.Add("validationErrors", result.ValidationErrors);
            
        return problemDetails;

    }
}