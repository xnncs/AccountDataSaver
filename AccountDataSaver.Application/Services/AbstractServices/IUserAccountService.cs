using AccountDataSaver.Application.Contracts;
using AccountDataSaver.Application.Models;
using AccountDataSaver.Core.Models;

namespace AccountDataSaver.Application.Services.AbstractServices;

public interface IUserAccountService
{
    Task AddAsync(AddAccountRequestModel requestModel);
    Task<IEnumerable<UserAccountModel>> GetAllAsync(string authorLogin);
    Task<UserAccountModel>? GetByServiceUrlAsync(string authorLogin, string serviceUrl);
}