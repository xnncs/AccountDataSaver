using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccountDataSaver.Core.Models;
using AccountDataSaver.Infrastructure.Abstract;
using AccountDataSaver.Infrastructure.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AccountDataSaver.Infrastructure.Helpers;

public class JwtProvider : IJwtProvider
{
    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value; 
    }
    
    private readonly JwtOptions _options;
    
    public string GenerateToken(UserModel user)
    {
        Claim[] data =
        {
            new Claim(nameof(user.Login), user.Login),
        };

        byte[] secretKeyBytes = Encoding.UTF8.GetBytes(_options.SecretKey);
        SymmetricSecurityKey key = new SymmetricSecurityKey(secretKeyBytes);
        
        SigningCredentials singingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        DateTime timeExpires = DateTime.UtcNow.AddHours(_options.ExpiresHours);
        
        JwtSecurityToken token = new JwtSecurityToken(
            claims: data,
            signingCredentials: singingCredentials,
            expires: timeExpires
        );

        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenValue;
    }
    
}