namespace Domain.Exceptions;

public sealed class NotFoundException(string message, params object[] args) : Exception(message)
{
    public IEnumerable<object> Args { get; set; } = args;
}