namespace AccountDataSaver.Infrastructure.Models;

public record PasswordHashOptions
{
    public int WorkFactor { get; set; }
}