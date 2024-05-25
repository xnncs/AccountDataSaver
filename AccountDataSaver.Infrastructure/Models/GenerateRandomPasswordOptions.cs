using System.ComponentModel.DataAnnotations;

namespace AccountDataSaver.Infrastructure.Models;

public record GenerateRandomPasswordOptions
{
    [Required]
    public bool ToGenerateRandomPassword { get; set; }
    
    [Range(6, 30)]
    public int PasswordLength { get; set; }
    
    [Range(0, 1)]
    public double NumbersRate { get; set; }
    
    [Range(0, 1)]
    public double SpecificSymbolsRate { get; set; }

    
}