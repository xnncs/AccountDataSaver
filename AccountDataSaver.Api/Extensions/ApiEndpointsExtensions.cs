using System.Text;
using AccountDataSaver.Api.Endpoints;
using AccountDataSaver.Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AccountDataSaver.Api.Extensions;

public static class ApiEndpointsExtensions
{
    public static void AddMappedEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapSaveAccountEndpoints();
        app.MapAuthEndpoints();
    }
}