using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;

namespace CleanArchitecture.Application.Tests.Integration.TodoItems.Commands;

public class CreateTodoItemTests : IClassFixture<CustomApiFactory>
{
    private readonly CustomApiFactory _apiFactory;

    public CreateTodoItemTests(CustomApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
    }
    
    [Fact]
    public async Task CreateTodoItem_ShouldRequireMinimumFields_WhenGivenAnEmptyCommandObject()
    {
        var command = new CreateTodoItemCommand();

        await FluentActions.Invoking(() => _apiFactory.SendAsync(command)).Should().ThrowAsync<AppValidationException>();
    }

    [Fact]
    public async Task CreateTodoItem_ShouldSuccess_WhenGivenAValidCommandObject()
    {
        // Arrange
        var userId = await _apiFactory.RunAsDefaultUserAsync();

        var listId = await _apiFactory.SendAsync(new CreateTodoListCommand { Title = "New List" });

        var command = new CreateTodoItemCommand { ListId = listId, Title = "Tasks" };

        // Act
        var itemId = await _apiFactory.SendAsync(command);

        var item = await _apiFactory.FindAsync<TodoItem>(itemId);

        // Assert
        item.Should().NotBeNull();
        item!.ListId.Should().Be(command.ListId);
        item.Title.Should().Be(command.Title);
        item.CreatedBy.Should().Be(userId);
        item.CreatedUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
        item.LastModifiedBy.Should().Be(userId);
        item.LastModifiedUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
    }
}