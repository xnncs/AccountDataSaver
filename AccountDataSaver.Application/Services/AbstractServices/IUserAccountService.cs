using AccountDataSaver.Application.Contracts;
using AccountDataSaver.Application.Models;
using AccountDataSaver.Core.Models;

namespace AccountDataSaver.Application.Services.AbstractServices;

public interface IUserAccountService
{
    Task AddAsync(AddAccountRequestModel requestModel);
    Task<IQueryable<UserAccountModel>> GetAllAsync(string authorLogin);
    Task UpdateByIdAsync(UpdateAccountRequestModel request);
}