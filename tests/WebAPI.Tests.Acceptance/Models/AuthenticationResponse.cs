namespace CleanArchitecture.WebApi.Tests.Acceptance.Models;

public class AuthenticationResponse
{
    public UserResult UserResult { get; set; } = null!;

    public string Token { get; set; } = null!;
}