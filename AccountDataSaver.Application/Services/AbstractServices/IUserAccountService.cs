using AccountDataSaver.Application.Contracts;
using AccountDataSaver.Application.Models;
using AccountDataSaver.Core.Models;

namespace AccountDataSaver.Application.Services.AbstractServices;

public interface IUserAccountService
{
    Task AddAsync(AddAccountRequestModel requestModel);
    IQueryable<UserAccountModel> GetAll(string authorLogin);
    Task UpdateAsync(UpdateAccountRequestModel request);
    Task DeleteAsync(DeleteAccountRequestModel request);
}