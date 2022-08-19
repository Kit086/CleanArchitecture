using System.Runtime.Serialization;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoLists.Queries.GetTodos;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Tests.Unit.Common.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config 
            => config.AddProfile<MappingProfile>());

        _mapper = _configuration.CreateMapper();
    }

    [Fact]
    public void AssertConfigurationIsValid_ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Theory]
    [InlineData(typeof(TodoList), typeof(TodoListDto))]
    [InlineData(typeof(TodoItem), typeof(TodoItemDto))]
    [InlineData(typeof(TodoList), typeof(LookupDto))]
    [InlineData(typeof(TodoItem), typeof(LookupDto))]
    public void Map_ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);
        
        _mapper.Map(instance, source, destination);
    }

    private static object GetInstanceOf(Type type)
    {
        return type.GetConstructor(Type.EmptyTypes) is not null 
            ? Activator.CreateInstance(type)! 
            : FormatterServices.GetUninitializedObject(type); // Type without parameterless constructor
    }
}