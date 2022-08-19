using CleanArchitecture.Application.TodoLists.Queries.GetTodos;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.ValueObjects;
using FluentAssertions;

namespace CleanArchitecture.Application.Tests.Integration.TodoLists.Queries;

public class GetTodosTests : CustomApiFactory
{
    // private readonly CustomApiFactory _apiFactory;
    //
    // public GetTodosTests(CustomApiFactory apiFactory)
    // {
    //     _apiFactory = apiFactory;
    // }
    
    [Fact]
    public async Task GetTodos_ShouldReturnPriorityLevel()
    {
        // Arrange
        await RunAsDefaultUserAsync();

        var query = new GetTodosQuery();
        
        // Act
        var result = await SendAsync(query);

        // Assert
        result.PriorityLevels.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetTodos_ShouldReturnAllListsAndItems()
    {
        // Arrange
        await RunAsDefaultUserAsync();

        await AddAsync(new TodoList
        {
            Title = "Shopping",
            Colour = Colour.Blue,
            Items =
            {
                new TodoItem { Title = "Apples", Done = true },
                new TodoItem { Title = "Milk", Done = true },
                new TodoItem { Title = "Bread", Done = true },
                new TodoItem { Title = "Toilet paper" },
                new TodoItem { Title = "Pasta" },
                new TodoItem { Title = "Tissues" },
                new TodoItem { Title = "Tuna" }
            }
        });

        var query = new GetTodosQuery();
        
        // Act
        var result = await SendAsync(query);
        
        // Assert
        result.Lists.Should().HaveCount(1);
        result.Lists.First().Items.Should().HaveCount(7);
    }

    [Fact]
    public async Task GetTodos_ShouldDeny_WhenAnonymousUser()
    {
        // Arrange
        var query = new GetTodosQuery();
        
        // Act
        var action = () => SendAsync(query);
        
        // Assert
        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}