using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;

namespace CleanArchitecture.Application.Tests.Integration.TodoLists.Commands;

public class CreateTodoListTests : IClassFixture<CustomApiFactory>
{
    private readonly CustomApiFactory _apiFactory;

    public CreateTodoListTests(CustomApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
    }
    
    [Fact]
    public async Task CreateTodoList_ShouldRequireMinimumFields_WhenGivenAnInvalidTodoList()
    {
        var command = new CreateTodoListCommand();

        await FluentActions.Invoking(() => _apiFactory.SendAsync(command)).Should().ThrowAsync<AppValidationException>();
    }

    [Fact]
    public async Task CreateTodoList_ShouldRequireUniqueTitle_WhenGivenAnExistentTitle()
    {
        await _apiFactory.SendAsync(new CreateTodoListCommand { Title = "Shopping" });

        var command = new CreateTodoListCommand { Title = "Shopping" };

        await FluentActions.Invoking(() => _apiFactory.SendAsync(command)).Should().ThrowAsync<AppValidationException>();
    }

    [Fact]
    public async Task CreateTodoList_ShouldSuccess_WhenGivenAValidCommandObject()
    {
        // Arrange
        var userId = await _apiFactory.RunAsDefaultUserAsync();

        var command = new CreateTodoListCommand { Title = "Tasks" };

        // Act
        var id = await _apiFactory.SendAsync(command);

        var list = await _apiFactory.FindAsync<TodoList>(id);

        // Assert
        list.Should().NotBeNull();
        list!.Title.Should().Be(command.Title);
        list.CreatedBy.Should().Be(userId);
        list.CreatedUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
    }
}