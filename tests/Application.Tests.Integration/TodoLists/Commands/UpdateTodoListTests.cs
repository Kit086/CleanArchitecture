using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;

namespace CleanArchitecture.Application.Tests.Integration.TodoLists.Commands;

public class UpdateTodoListTests : IClassFixture<CustomApiFactory>
{
    private readonly CustomApiFactory _apiFactory;

    public UpdateTodoListTests(CustomApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
    }
    
    [Fact]
    public async Task UpdateTodoList_ShouldRequireValidTodoListId_WhenGivenANotExistTodoListId()
    {
        var command = new UpdateTodoListCommand { Id = 99, Title = "New Title" };

        await FluentActions.Invoking(() => _apiFactory.SendAsync(command)).Should().ThrowAsync<AppNotFoundException>();
    }

    [Fact]
    public async Task UpdateTodoList_ShouldRequireUniqueTitle_WhenGivenAnExistTitle()
    {
        // Arrange
        var listId = await _apiFactory.SendAsync(new CreateTodoListCommand { Title = "New List" });

        await _apiFactory.SendAsync(new CreateTodoListCommand { Title = "Other List" });

        var command = new UpdateTodoListCommand { Id = listId, Title = "Other List" };

        // Assert
        (await FluentActions.Invoking(() => _apiFactory.SendAsync(command))
                .Should().ThrowAsync<AppValidationException>()
                .Where(ex => ex.Errors.ContainsKey("Title")))
            .And.Errors["Title"].Should()
            .Contain("The specified title already exists.");
    }

    [Fact]
    public async Task UpdateTodoList_ShouldSuccess()
    {
        // Arrange
        var userId = await _apiFactory.RunAsDefaultUserAsync();

        var listId = await _apiFactory.SendAsync(new CreateTodoListCommand { Title = "New List" });

        var command = new UpdateTodoListCommand { Id = listId, Title = "Updated List Title" };

        // Act
        await _apiFactory.SendAsync(command);

        var list = await _apiFactory.FindAsync<TodoList>(listId);
        
        // Assert
        list.Should().NotBeNull();
        list!.Title.Should().Be(command.Title);
        list.LastModifiedBy.Should().NotBeNull();
        list.LastModifiedBy.Should().Be(userId);
        list.LastModifiedUtc.Should().NotBeNull();
        list.LastModifiedUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
    }
}