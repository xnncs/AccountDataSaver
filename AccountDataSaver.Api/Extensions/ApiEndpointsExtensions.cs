using System.Text;
using AccountDataSaver.Api.Endpoints;
using AccountDataSaver.Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AccountDataSaver.Api.Extensions;

public static class ApiEndpointsExtensions
{
    public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        JwtOptions jwtOtions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>() ?? throw new Exception("Wrong jwtOptions format");
        string secretKey = jwtOtions.SecretKey;
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["tasty-cookies"];

                        return Task.CompletedTask;
                    }
                };
            });
        
        services.AddAuthorization(options =>
        {

        });
    }

    public static void AddMappedEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapSaveAccountEndpoints();
        app.MapAuthEndpoints();
    }
}