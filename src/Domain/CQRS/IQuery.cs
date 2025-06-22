namespace Domain.CQRS;

public interface IQuery<out TResponse> : IQuery, IRequest<TResponse> where TResponse : notnull
{
    
}


public interface IQuery
{
    
}