using CleanArchitecture.Application.Common.Behaviours;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CleanArchitecture.Application.Tests.Unit.Common.Behaviors;

public class RequestLoggerTests
{
    private readonly ILogger<CreateTodoItemCommand> _logger = Substitute.For<ILogger<CreateTodoItemCommand>>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();

    [Fact]
    public async Task ShouldCallGetUserNameAsyncOnce_WhenAuthenticated()
    {
        // Arrange
        _currentUserService.UserId.Returns(Guid.NewGuid().ToString());
        var requestLogger = new LoggingBehaviour<CreateTodoItemCommand>(_logger, _currentUserService, _identityService);
        
        // Act
        await requestLogger.Process(new CreateTodoItemCommand { ListId = 1, Title = "title" }, new CancellationToken());
        
        // Assert
        await _identityService.Received(1).GetUserNameAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task ShouldNotCallGetUserNameAsyncOnce_WhenUnauthenticated()
    {
        // Arrange
        var requestLogger = new LoggingBehaviour<CreateTodoItemCommand>(_logger, _currentUserService, _identityService);
        
        // Act
        await requestLogger.Process(new CreateTodoItemCommand { ListId = 1, Title = "title" }, new CancellationToken());
        
        // Assert
        await _identityService.Received(0).GetUserNameAsync(Arg.Any<string>());
    }
}