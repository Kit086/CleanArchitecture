using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Application.TodoLists.Commands.DeleteTodoList;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;

namespace CleanArchitecture.Application.Tests.Integration.TodoLists.Commands;

public class DeleteTodoListTests : IClassFixture<CustomApiFactory>
{
    private readonly CustomApiFactory _apiFactory;

    public DeleteTodoListTests(CustomApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
    }
    
    [Fact]
    public async Task DeleteTodoList_ShouldRequireValidTodoListId_WhenGivenAnNotExistTodoListId()
    {
        var command = new DeleteTodoListCommand(99);

        await FluentActions.Invoking(() => _apiFactory.SendAsync(command)).Should().ThrowAsync<AppNotFoundException>();
    }

    [Fact]
    public async Task DeleteTodoList_ShouldSuccess_WhenGivenAnExistTodoListId()
    {
        // Arrange
        var listId = await _apiFactory.SendAsync(new CreateTodoListCommand { Title = "New List" });

        // Act
        await _apiFactory.SendAsync(new DeleteTodoListCommand(listId));

        var list = await _apiFactory.FindAsync<TodoList>(listId);

        // Assert
        list.Should().BeNull();
    }
}