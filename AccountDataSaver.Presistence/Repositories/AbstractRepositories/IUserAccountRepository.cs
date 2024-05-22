using AccountDataSaver.Core.Models;

namespace AccountDataSaver.Presistence.Repositories.AbstractRepositories;

public interface IUserAccountRepository
{
    Task AddAsync(UserAccountModel user);
    bool Contains(string serviceUrl, string login, string password);
    IQueryable<UserAccountModel> GetAllAccounts(string authorLogin);
    UserAccountModel GetAccountByUrl(string authorLogin, string serviceUrl);
}