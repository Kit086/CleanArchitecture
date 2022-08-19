using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItemDetail;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using FluentAssertions;

namespace CleanArchitecture.Application.Tests.Integration.TodoItems.Commands;

public class UpdateTodoItemDetailTests : IClassFixture<CustomApiFactory>
{
    private readonly CustomApiFactory _apiFactory;

    public UpdateTodoItemDetailTests(CustomApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
    }
    
    [Fact]
    public async Task UpdateTodoItemDetail_ShouldRequireValidTodoItemId_WhenGivenANotExistentTodoItem()
    {
        var command = new UpdateTodoItemCommand { Id = 99, Title = "New Title" };

        await FluentActions.Invoking(() => _apiFactory.SendAsync(command)).Should().ThrowAsync<AppNotFoundException>();
    }

    [Fact]
    public async Task UpdateTodoItemDetail_ShouldSuccess_WhenGivenAnExistentTodoItem()
    {
        // Arrange
        var userId = await _apiFactory.RunAsDefaultUserAsync();

        var listId = await _apiFactory.SendAsync(new CreateTodoListCommand { Title = "New List" });

        var itemId = await _apiFactory.SendAsync(new CreateTodoItemCommand { ListId = listId, Title = "New Item" });

        var command = new UpdateTodoItemDetailCommand
        {
            Id = itemId, ListId = listId, Note = "This is the note.", Priority = PriorityLevel.High
        };
        
        // Act
        await _apiFactory.SendAsync(command);
        
        var item = await _apiFactory.FindAsync<TodoItem>(itemId);
        
        // Assert
        item.Should().NotBeNull();
        item!.ListId.Should().Be(command.ListId);
        item.Note.Should().Be(command.Note);
        item.Priority.Should().Be(command.Priority);
        item.LastModifiedBy.Should().NotBeNull();
        item.LastModifiedBy.Should().Be(userId);
        item.LastModifiedUtc.Should().NotBeNull();
        item.LastModifiedUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
    }
}