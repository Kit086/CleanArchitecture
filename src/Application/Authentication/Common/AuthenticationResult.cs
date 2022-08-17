namespace CleanArchitecture.Application.Authentication.Common;

public class AuthenticationResult
{
    public AuthenticationResult(UserResult userResult, string token)
    {
        UserResult = userResult;
        Token = token;
    }
    public UserResult UserResult { get; }

    public string Token { get; }
}