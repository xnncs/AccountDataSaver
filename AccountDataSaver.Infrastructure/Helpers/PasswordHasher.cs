using AccountDataSaver.Core.Models;
using AccountDataSaver.Infrastructure.Abstract;
using AccountDataSaver.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AccountDataSaver.Infrastructure.Helpers;

public class PasswordHasher : IPasswordHasher
{
    public PasswordHasher(IOptions<PasswordHashOptions> options)
    {
        _options = options.Value;
    }

    private readonly PasswordHashOptions _options;

    public string HashPassword(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password, _options.WorkFactor);
    

    public PasswordVerificationResult VerifyHashedPassword(string providedPassword, string hashedPassword)
    {
        bool result = BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, hashedPassword);
        
        if (!result)
        {
            return PasswordVerificationResult.Failed;
        }
        return PasswordVerificationResult.Success;
    }
}