using AccountDataSaver.Application.Contracts;
using AccountDataSaver.Application.Models;

namespace AccountDataSaver.Application.Services.AbstractServices;

public interface IUserAccountService
{
    public Task AddAsync(AddAccountRequestModel requestModel);
}