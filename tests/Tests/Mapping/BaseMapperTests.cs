using Application.Mapping;
using Application.Models;
using AutoMapper;
using Domain.Abstraction;

namespace Tests.Mapping;

public abstract class BaseMapperTests : IDisposable
{
    protected IMapper TestMapper;
    
    public void Dispose()
    {
        // TODO release managed resources here
    }
    
    protected void CheckEntityMapping<TSource, TDestination>(TSource source, TDestination destination) where TSource : Entity where TDestination : BaseResponse
    {
        Assert.Equal(source.Id, destination.Id);
        Assert.Equal(source.CreatedAt, destination.CreatedAt);
        Assert.Equal(source.ModifiedAt, destination.ModifiedAt);
        Assert.Equal(source.IsActive, destination.IsActive);
    }
}