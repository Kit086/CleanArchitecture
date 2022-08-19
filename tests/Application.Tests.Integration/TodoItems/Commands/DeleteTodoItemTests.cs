using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.DeleteTodoItem;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;

namespace CleanArchitecture.Application.Tests.Integration.TodoItems.Commands;

public class DeleteTodoItemTests : IClassFixture<CustomApiFactory>
{
    private readonly CustomApiFactory _apiFactory;

    public DeleteTodoItemTests(CustomApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
    }
    
    [Fact]
    public async Task DeleteTodoItem_ShouldRequireValidTodoItemId_WhenGivenANoExistentTodoItem()
    {
        var command = new DeleteTodoItemCommand(99);

        await FluentActions.Invoking(() => _apiFactory.SendAsync(command))
            .Should().ThrowAsync<AppNotFoundException>();
    }

    [Fact]
    public async Task DeleteTodoItem_ShouldSuccess_WhenGivenAnExistentTodoItem()
    {
        // Arrange
        var listId = await _apiFactory.SendAsync(new CreateTodoListCommand { Title = "New List" });
        
        var itemId = await _apiFactory.SendAsync(new CreateTodoItemCommand { ListId = listId, Title = "New Item" });

        // Act
        await _apiFactory.SendAsync(new DeleteTodoItemCommand(itemId));
        
        var item = await _apiFactory.FindAsync<TodoItem>(itemId);
        
        // Assert
        item.Should().BeNull();
    }
}