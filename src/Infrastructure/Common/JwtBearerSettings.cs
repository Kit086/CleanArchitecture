namespace CleanArchitecture.Infrastructure.Common;

public class JwtBearerSettings
{
    public string Audience { get; set; } = null!;
    
    public string Issuer { get; set; } = null!;
    
    public string IssuerSigningKey { get; set; } = null!;
    
    public int ExpiryMinutes { get; set; }
}