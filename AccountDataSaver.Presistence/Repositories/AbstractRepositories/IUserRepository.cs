using AccountDataSaver.Core.Models;
using AccountDataSaver.Presistence.Entities;

namespace AccountDataSaver.Presistence.Repositories.AbstractRepositories;

public interface IUserRepository
{
    Task AddAsync(UserModel user);
    UserModel GetByLogin(string login);
    bool ContainsByLogin(string login);
    UserModel GetById(int id);
}