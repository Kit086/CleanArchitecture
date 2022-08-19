using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItem;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;

namespace CleanArchitecture.Application.Tests.Integration.TodoItems.Commands;

public class UpdateTodoItemTests : IClassFixture<CustomApiFactory>
{
    private readonly CustomApiFactory _apiFactory;

    public UpdateTodoItemTests(CustomApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
    }
    
    [Fact]
    public async Task UpdateTodoItem_ShouldRequireValidTodoItemId_WhenGivenANoExistentTodoItem()
    {
        var command = new UpdateTodoItemCommand { Id = 99, Title = "New Title" };

        await FluentActions.Invoking(() => _apiFactory.SendAsync(command))
            .Should().ThrowAsync<AppNotFoundException>();
    }

    [Fact]
    public async Task UpdateTodoItem_ShouldSuccess_WhenGivenAnExistentTodoItem()
    {
        // Arrange
        var userId = await _apiFactory.RunAsDefaultUserAsync();

        var listId = await _apiFactory.SendAsync(new CreateTodoListCommand { Title = "New List" });

        var itemId = await _apiFactory.SendAsync(new CreateTodoItemCommand { ListId = listId, Title = "New Item" });

        var command = new UpdateTodoItemCommand { Id = itemId, Title = "Update Item Title" };
        
        // Act
        await _apiFactory.SendAsync(command);

        var item = await _apiFactory.FindAsync<TodoItem>(itemId);
        
        // Assert
        item.Should().NotBeNull();
        item!.Title.Should().Be(command.Title);
        item.LastModifiedBy.Should().NotBeNull();
        item.LastModifiedBy.Should().Be(userId);
        item.LastModifiedUtc.Should().NotBeNull();
        item.LastModifiedUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
    }
}