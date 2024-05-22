namespace AccountDataSaver.Infrastructure.Models;

public record GenerateRandomPasswordOptions(bool ToGenerateRandomPassword,
    int PasswordLength,
    double NumbersRate,
    double SpecificSymbolsRate
    );