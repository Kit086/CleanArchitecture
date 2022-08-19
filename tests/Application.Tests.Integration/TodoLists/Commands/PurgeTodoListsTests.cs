using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Application.TodoLists.Commands.PurgeTodoLists;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;

namespace CleanArchitecture.Application.Tests.Integration.TodoLists.Commands;

public class PurgeTodoListsTests : CustomApiFactory
{
    // private readonly CustomApiFactory _apiFactory;
    //
    // public PurgeTodoListsTests(CustomApiFactory apiFactory)
    // {
    //     _apiFactory = apiFactory;
    // }
    
    [Fact]
    public async Task PurgeTodoLists_ShouldDeny_WhenAnonymousUser()
    {
        // Arrange
        var command = new PurgeTodoListsCommand();

        // Assert
        command.GetType().Should().BeDecoratedWith<CleanArchitecture.Application.Common.Security.AppAuthorizeAttribute>();
        
        // Act
        var action = () => SendAsync(command);

        // Assert
        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task PurgeTodoLists_ShouldDeny_WhenUserRoleIsNotAdministrator()
    {
        // Arrange
        await RunAsDefaultUserAsync();

        var command = new PurgeTodoListsCommand();

        // Act
        var action = () => SendAsync(command);
        
        // Assert
        await action.Should().ThrowAsync<AppForbiddenAccessException>();
    }

    [Fact]
    public async Task PurgeTodoLists_ShouldAllow_WhenUserRoleIsAdministrator()
    {
        // Arrange
        await RunAsAdministratorAsync();

        var command = new PurgeTodoListsCommand();
        
        // Act
        var action = () => SendAsync(command);
        
        // Assert
        await action.Should().NotThrowAsync<AppForbiddenAccessException>();
    }

    [Fact]
    public async Task PurgeTodoLists_ShouldDeleteAllLists_WhenUserRoleIsAdministrator()
    {
        // Arrange
        await RunAsAdministratorAsync();
        
        await SendAsync(new CreateTodoListCommand
        {
            Title = "New List #1"
        });
        await SendAsync(new CreateTodoListCommand
        {
            Title = "New List #2"
        });
        await SendAsync(new CreateTodoListCommand
        {
            Title = "New List #3"
        });
        
        // Act
        await SendAsync(new PurgeTodoListsCommand());

        var count = await CountAsync<TodoList>();
        
        // Assert
        count.Should().Be(0);
    }
}