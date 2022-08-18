using AutoMapper;
using CleanArchitecture.Application.Authentication.Common;
using CleanArchitecture.Application.Authentication.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email, 
    string Password) : IRequest<AuthenticationResult>;

public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;
    
    public LoginQueryHandler(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager, 
        IJwtTokenGenerator jwtTokenGenerator,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
    }
    
    public async Task<AuthenticationResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
        {
            throw new AppAuthenticationException("The email or password is incorrect.");
        }
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
    
        if (!result.Succeeded)
        {
            // TODO: The meaning of the message is vague
            throw new AppAuthenticationException(result.ToString());
        }
    
        return new AuthenticationResult(
            _mapper.Map<UserResult>(user),
            _jwtTokenGenerator.GenerateToken(user));
    }
}