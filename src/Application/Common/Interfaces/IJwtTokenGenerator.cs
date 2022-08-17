using CleanArchitecture.Identity.Models;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(ApplicationUser user);
}