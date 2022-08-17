using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Identity.Models;
using CleanArchitecture.Infrastructure.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Infrastructure.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtBearerSettings _jwtBearerSettings;
    private readonly IDateTime _dateTime;

    public JwtTokenGenerator(IOptions<JwtBearerSettings> jwtBearerSettings, IDateTime dateTime)
    {
        _dateTime = dateTime;
        _jwtBearerSettings = jwtBearerSettings.Value;
    }

    public string GenerateToken(ApplicationUser user)
    {
        var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtBearerSettings.IssuerSigningKey));
        var signingCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id), 
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var expires = _dateTime.UtcNow.AddMinutes(_jwtBearerSettings.ExpiryMinutes);
        var token = new JwtSecurityToken(
            _jwtBearerSettings.Issuer,
            _jwtBearerSettings.Audience,
            claims,
            expires: expires,
            signingCredentials: signingCredentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}