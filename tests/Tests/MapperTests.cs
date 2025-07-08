using Application.Mapping;
using Application.Models.Employee;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

public class MapperTests : IDisposable
{
    private readonly AutoMapper.IConfigurationProvider _mapperConfig;
    private readonly IMapper mapper;
    public MapperTests()
    {
        _mapperConfig = new MapperConfiguration(x =>
        {
            x.AddProfile<EmployeeProfile>();
        });

        mapper = _mapperConfig.CreateMapper();
  
    }

    [Fact]
    public void AutoMapper_Configuration_IsValid()
    {
        _mapperConfig.AssertConfigurationIsValid();
    }

    [Fact]
    public void TestEmployeeMappingToDomainModel()
    {
        var employee = new DatabaseFaker().GenerateEmployees(1).First();

        var mappedEmployee = mapper.Map<EmployeeResponse>(employee);

        Assert.NotNull(mappedEmployee);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
