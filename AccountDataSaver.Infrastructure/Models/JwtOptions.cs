namespace AccountDataSaver.Infrastructure.Models;

public record JwtOptions
{
    public string SecretKey { get; set; }
    public int ExpiresHours { get; set; } 
}