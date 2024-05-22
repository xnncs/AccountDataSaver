using AccountDataSaver.Application.Contracts;
using AccountDataSaver.Infrastructure.Abstract;
using Microsoft.AspNetCore.Http;

namespace AccountDataSaver.Application.Services.AbstractServices;

public interface IUserService
{
    Task RegisterAsync(RegisterUserRequestModel requestModel);
    Task<string> LoginAsync(LoginUserRequestModel requestModel);
    string GetLoginFromJwt(HttpContext context, IJwtProvider jwtProvider);
}