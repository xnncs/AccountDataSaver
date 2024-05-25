using AccountDataSaver.Api.Extensions;
using AccountDataSaver.Application.Services.AbstractServices;
using AccountDataSaver.Application.Services.RealisationServices;
using AccountDataSaver.Core.Models;
using AccountDataSaver.Infrastructure.Abstract;
using AccountDataSaver.Infrastructure.Helpers;
using AccountDataSaver.Infrastructure.Models;
using AccountDataSaver.Presistence;
using AccountDataSaver.Presistence.Repositories.AbstractRepositories;
using AccountDataSaver.Presistence.Repositories.RealisationRepositories;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// configuring options 
services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.Configure<PasswordHashOptions>(configuration.GetSection(nameof(PasswordHashOptions)));

services.AddSwaggerGen();

services.AddLogging();

services.AddApiAuthentication(configuration);

// adding mappers
services.AddAutoMapper(typeof(AppMappingProfile));

// adding services for minimal api mapping
services.AddEndpointsApiExplorer();

// adding helpers 
services.AddSingleton<IPasswordHasher, PasswordHasher>();
services.AddSingleton<IJwtProvider, JwtProvider>();
services.AddSingleton<IPasswordHelper, PasswordHelper>();

// adding business logic services 
services.AddScoped<IAuthorizationService, AuthorizationService>();
services.AddScoped<IUserAccountService, UserAccountService>();

// adding dto repositories
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IUserAccountRepository, UserAccountRepository>();

// adding db contexts
services.AddDbContext<ApplicationDbContext>(options =>
{
    string? connectionString = configuration.GetConnectionString(nameof(ApplicationDbContext));
    if (connectionString == null)
    {
        throw new Exception("Connection string does not exist");
    }

    options.UseNpgsql(connectionString);
});

var app = builder.Build();

// adding swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.AddMappedEndpoints();

app.UseHttpsRedirection();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              

app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();

app.Run();