using AccountDataSaver.Core.Models;

namespace AccountDataSaver.Infrastructure.Abstract;

public interface IJwtProvider
{
    public string GenerateToken(UserModel user);
    public string DecodeToken(string token);
}