using AccountDataSaver.Infrastructure.Models;

namespace AccountDataSaver.Infrastructure.Abstract;

public interface IPasswordHelper
{
    public string GenerateRandomPassword(GenerateRandomPasswordOptions options);
}