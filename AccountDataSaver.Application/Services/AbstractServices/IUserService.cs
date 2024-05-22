using AccountDataSaver.Application.Contracts;

namespace AccountDataSaver.Application.Services.AbstractServices;

public interface IUserService
{
    public Task RegisterAsync(RegisterUserRequestModel requestModel);
    public Task<string> LoginAsync(LoginUserRequestModel requestModel);
}