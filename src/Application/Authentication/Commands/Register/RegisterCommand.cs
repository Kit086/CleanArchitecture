using AutoMapper;
using CleanArchitecture.Application.Authentication.Common;
using CleanArchitecture.Application.Authentication.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string UserName,
    string Email,
    string Password) : IRequest<AuthenticationResult>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    
    public RegisterCommandHandler(UserManager<ApplicationUser> userManager, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _mapper = mapper;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email
        };
    
        var result = await _userManager.CreateAsync(user, request.Password);
    
        if (!result.Succeeded)
        {
            throw new AuthException(result.Errors);
        }
    
        return new AuthenticationResult(
            _mapper.Map<UserResult>(user),
            _jwtTokenGenerator.GenerateToken(user));
    }
}