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
        Claim[] claims =
        {
            new Claim(nameof(user.Login), user.Login),
        };

        byte[] secretKeyBytes = Encoding.UTF8.GetBytes(_options.SecretKey);
        SymmetricSecurityKey key = new SymmetricSecurityKey(secretKeyBytes);
        
        SigningCredentials singingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        DateTime timeExpires = DateTime.UtcNow.AddHours(_options.ExpiresHours);
        
        JwtSecurityToken token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: singingCredentials,
            expires: timeExpires
        );

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        
        string tokenValue = handler.WriteToken(token);
        return tokenValue;
    }

    public string GetLoginFromClaims(string jwtToken)
    {
        byte[] secretKeyBytes = Encoding.UTF8.GetBytes(_options.SecretKey);
        
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        
        TokenValidationParameters validations = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
            ValidateIssuer = false,
            ValidateAudience = false
        };
        ClaimsPrincipal claims = handler.ValidateToken(jwtToken, validations, out var tokenSecure);

        return claims.FindFirst(x => x.Type == nameof(UserModel.Login))!.Value;
    }
}