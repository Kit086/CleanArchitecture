using CleanArchitecture.Application.Authentication.Commands.Register;
using CleanArchitecture.Application.Authentication.Common;
using CleanArchitecture.Application.Authentication.Queries.Login;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebAPI.Controllers;

public class AuthenticationController : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var authResult = await Mediator.Send(command);

        return Ok(authResult);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginQuery command)
    {
        var authResult = await Mediator.Send(command);

        return Ok(authResult);
    }
}