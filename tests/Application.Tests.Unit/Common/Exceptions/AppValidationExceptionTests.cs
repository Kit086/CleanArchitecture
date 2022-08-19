using CleanArchitecture.Application.Common.Exceptions;
using FluentAssertions;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace CleanArchitecture.Application.Tests.Unit.Common.Exceptions;

public class AppValidationExceptionTests
{
    [Fact]
    public void DefaultConstructor_ShouldCreatesAnEmptyErrorDictionary()
    {
        // Arrange
        // Act
        var actual = new AppValidationException().Errors;

        // Assert
        actual.Keys.Should().BeEquivalentTo(Array.Empty<string>());
    }

    [Fact]
    public void SingleValidationFailure_ShouldCreatesASingleElementErrorDictionary()
    {
        // Arrange
        var failures = new List<ValidationFailure> { new ValidationFailure("Age", "must be over 18"), };

        // Act
        var actual = new AppValidationException(failures).Errors;

        // Assert
        actual.Keys.Should().BeEquivalentTo(new string[] { "Age" });
        actual["Age"].Should().BeEquivalentTo(new string[] { "must be over 18" });
    }

    [Fact]
    public void
        MultipleValidationFailure_ShouldForMultiplePropertiesCreatesAMultipleElementErrorDictionaryEachWithMultipleValues()
    {
        // Arrange
        var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Age", "must be 18 or older"),
            new ValidationFailure("Age", "must be 25 or younger"),
            new ValidationFailure("Password", "must contain at least 8 characters"),
            new ValidationFailure("Password", "must contain a digit"),
            new ValidationFailure("Password", "must contain upper case letter"),
            new ValidationFailure("Password", "must contain lower case letter"),
        };

        // Act
        var actual = new AppValidationException(failures).Errors;

        // Assert
        actual.Keys.Should().BeEquivalentTo(new string[]
        {
            "Password", 
            "Age"
        });
        actual["Age"].Should().BeEquivalentTo(new string[]
        {
            "must be 18 or older", "must be 25 or younger"
        });
        actual["Password"].Should().BeEquivalentTo(new string[]
        {
            "must contain lower case letter", 
            "must contain upper case letter",
            "must contain at least 8 characters", 
            "must contain a digit"
        });
    }
}