using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Authentication.Common.Exceptions;

public class AppAuthenticationException : Exception
{
    public AppAuthenticationException() : base("One or more authentication errors have occurred.")
    {
        Errors = new Dictionary<string, string>();
    }

    public AppAuthenticationException(string message): base(message)
    {
        Errors = new Dictionary<string, string> { { "Authentication Error", message } };
    }
    
    public AppAuthenticationException(IEnumerable<IdentityError> errors) : this()
    {
        Errors = errors.ToDictionary(e => e.Code, e => e.Description);
    }

    public IDictionary<string, string> Errors { get; }
}