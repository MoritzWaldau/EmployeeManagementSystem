using FluentValidation.Results;

namespace Application.Pattern;

public class Result
{
    public bool IsSuccess { get; protected init; }
    public string ErrorMessage { get; protected init; } = string.Empty;
    public List<ValidationFailure> ValidationErrors { get; protected init; } = [];
    
    public static Result Failure(string errorMessage)
    {
        return new Result
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
    
    public static Result Failure(string errorMessage, List<ValidationFailure> validationFailures)
    {
        return new Result
        {
            IsSuccess = false,
            ValidationErrors = validationFailures,
            ErrorMessage = errorMessage
        };
    }

    public static Result Success()
    {
        return new Result
        {
            IsSuccess = true
        };
    }
    
    public TR Match<TR>(Func<TR> onSuccess, Func<string, TR> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(ErrorMessage);
    }
}

public class Result<T> : Result
{
    public T? Value { get; private init;}
    public bool HasValue => Value is not null;
    
    public static Result<T> Success(T value)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Value = value,
        };
    }
    
    public new static Result<T> Success()
    {
        return new Result<T>
        {
            IsSuccess = true,
        };
    }
    
    public new static Result<T> Failure(string errorMessage)
    {
        return new Result<T>
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
        };
    }
    
    public static Result<T> Failure(List<ValidationFailure> validationFailures)
    {
        return new Result<T>
        {
            IsSuccess = false,
            ValidationErrors = validationFailures
        };
    }
    
    public TR Match<TR>(Func<T, TR> onSuccess, Func<Result<T>, TR> onFailure)
    {
        return IsSuccess ? onSuccess(Value!) : onFailure(this);
    }
}



    
