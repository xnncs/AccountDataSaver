using AccountDataSaver.Core.Models;

namespace AccountDataSaver.Presistence.Repositories.AbstractRepositories;

public interface IUserAccountRepository
{
    Task AddAsync(UserAccountModel user);
    bool Contains(string serviceUrl, string login, string password);
    IQueryable<UserAccountModel> GetAllAccounts(string authorLogin);
    Task UpdateAsync(int accountId, UserAccountModel model);
    string GetAuthorLoginByAccountId(int id);
}