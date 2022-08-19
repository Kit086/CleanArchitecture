using CleanArchitecture.Application.Authentication.Common.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Tests.Unit.Authentication.Common.Exceptions;

public class AppAuthenticationExceptionTests
{
    [Fact]
    public void DefaultConstructor_ShouldCreatesAnEmptyErrorDictionary()
    {
        // Arrange
        // Act
        var actual = new AppAuthenticationException().Errors;

        // Assert
        actual.Keys.Should().BeEquivalentTo(Array.Empty<string>());
    }

    [Fact]
    public void SingleValidationFailure_ShouldCreatesASingleElementErrorDictionary()
    {
        // Arrange
        var identityErrors = new List<IdentityError>
        {
            new IdentityError { Code = "code", Description = "Email must not be empty or whitespace" },
        };

        // Act
        var actual = new AppAuthenticationException(identityErrors).Errors;

        // Assert
        actual.Keys.Should().BeEquivalentTo(new string[] { "code" });
        actual["code"].Should().BeEquivalentTo("Email must not be empty or whitespace");
    }

    [Fact]
    public void
        MultipleAuthErrors_ShouldForMultiplePropertiesCreatesAMultipleElementErrorDictionaryEachWithMultipleValues()
    {
        // Arrange
        var identityErrors = new List<IdentityError>
        {
            new IdentityError { Code = "code", Description = "Email must not be empty or whitespace" },
            new IdentityError { Code = "code2", Description = "Password must not be empty or whitespace" }
        };

        // Act
        var actual = new AppAuthenticationException(identityErrors).Errors;

        // Assert
        actual.Keys.Should().BeEquivalentTo(new string[] { "code", "code2" });
        actual["code"].Should().BeEquivalentTo("Email must not be empty or whitespace");
        actual["code2"].Should().BeEquivalentTo("Password must not be empty or whitespace");
    }
}