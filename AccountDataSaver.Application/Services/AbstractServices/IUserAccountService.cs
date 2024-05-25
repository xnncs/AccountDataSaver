using AccountDataSaver.Application.Contracts;
using AccountDataSaver.Application.Models;
using AccountDataSaver.Core.Models;

namespace AccountDataSaver.Application.Services.AbstractServices;

public interface IUserAccountService
{
    Task AddAsync(AddAccountRequestModel requestModel);
    Task UpdateAsync(UpdateAccountRequestModel request);
    Task DeleteAsync(DeleteAccountRequestModel request);
    IEnumerable<UserAccountModel> GetByUrl(string authorLogin, string url);
    IEnumerable<UserAccountModel> GetAll(string authorLogin);
}