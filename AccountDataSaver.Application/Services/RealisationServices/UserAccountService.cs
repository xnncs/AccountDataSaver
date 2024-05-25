using AccountDataSaver.Application.Contracts;
using AccountDataSaver.Application.Models;
using AccountDataSaver.Application.Services.AbstractServices;
using AccountDataSaver.Core.Models;
using AccountDataSaver.Infrastructure.Abstract;
using AccountDataSaver.Presistence.Repositories.AbstractRepositories;

namespace AccountDataSaver.Application.Services.RealisationServices;

public class UserAccountService : IUserAccountService
{
    public UserAccountService(IUserAccountRepository accountRepository, IUserRepository userRepository)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
    }
    
    private readonly IUserAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    
    
    public async Task UpdateAsync(UpdateAccountRequestModel request)
    {
        CheckAuthorPermissions(request.AccountId, request.RequestUserLogin);

        await _accountRepository.UpdateAsync(request.AccountId, request.Data);
    }

    public async Task DeleteAsync(DeleteAccountRequestModel request)
    {
        CheckAuthorPermissions(request.AccountId, request.RequestUserLogin);

        await _accountRepository.DeleteAsync(request.AccountId);
    }
    
    public async Task AddAsync(AddAccountRequestModel request)
    {
        if (!_userRepository.ContainsByLogin(request.AuthorLogin))
        {
            throw new Exception("No such author with that login");
        }
        
        if (_accountRepository
            .Contains(
                login: request.Login,
                password: request.Password,
                serviceUrl: request.ServiceUrl)
            )
        {
            throw new Exception("Already saved account with such login and password");
        }

        UserAccountModel account = UserAccountModel.Create( 
            serviceUrl: request.ServiceUrl,
            description: request.Description,
            login: request.Login,
            password: request.Password,
            authorLogin: request.AuthorLogin
        );

        await _accountRepository.AddAsync(account);
    }

    public IEnumerable<UserAccountModel> GetAll(string authorLogin)
    {
        IQueryable<UserAccountModel>? accounts = _accountRepository.GetAllAccounts(authorLogin);
        if (accounts == null)
        {
            return Enumerable.Empty<UserAccountModel>();
        }
        
        return accounts.AsEnumerable();
    }

    public IEnumerable<UserAccountModel> GetByUrl(string authorLogin, string url)
    {
        IQueryable<UserAccountModel>? accounts = _accountRepository.GetAllAccounts(authorLogin);
        if (accounts == null)
        {
            return Enumerable.Empty<UserAccountModel>();
        }

        return accounts.Where(x => x.ServiceUrl == url).AsEnumerable();
    }


    // throws exception if user has no author permissions
    private void CheckAuthorPermissions(int requestAccountId, string clientLogin)
    {
        string authorLogin = _accountRepository.GetAuthorLoginByAccountId(requestAccountId);
        if (authorLogin != clientLogin)
        {
            throw new Exception(
                "you have no permissions to change that account because you are not that account author");
        }
    }
}