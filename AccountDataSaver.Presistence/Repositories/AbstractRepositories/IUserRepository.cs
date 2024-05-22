using AccountDataSaver.Core.Models;
using AccountDataSaver.Presistence.Entities;

namespace AccountDataSaver.Presistence.Repositories.AbstractRepositories;

public interface IUserRepository
{
    Task AddAsync(UserModel user);
    Task<UserModel>? GetByLoginAsync(string login);
    bool ContainsByLogin(string login);

}