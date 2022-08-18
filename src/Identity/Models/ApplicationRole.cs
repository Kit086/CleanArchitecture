using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Identity.Models;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole() : base()
    {
        
    }

    public ApplicationRole(string roleName) : base(roleName)
    {
        
    }
}