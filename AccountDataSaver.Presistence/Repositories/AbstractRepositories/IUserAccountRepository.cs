using AccountDataSaver.Core.Models;

namespace AccountDataSaver.Presistence.Repositories.AbstractRepositories;

public interface IUserAccountRepository
{
    Task AddAsync(UserAccountModel user);
    bool Contains(string serviceUrl, string login, string password);
}