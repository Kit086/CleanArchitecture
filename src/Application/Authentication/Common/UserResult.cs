using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Identity.Models;

namespace CleanArchitecture.Application.Authentication.Common;

public class UserResult : IMapFrom<ApplicationUser>
{
    public string Id { get; set; } = null!;
    
    public string UserName { get; init; } = null!;

    public string Email { get; init; } = null!;
}