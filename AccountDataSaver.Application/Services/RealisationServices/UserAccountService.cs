using AccountDataSaver.Application.Contracts;
using AccountDataSaver.Application.Models;
using AccountDataSaver.Application.Services.AbstractServices;
using AccountDataSaver.Core.Models;
using AccountDataSaver.Infrastructure.Abstract;
using AccountDataSaver.Presistence.Repositories.AbstractRepositories;

namespace AccountDataSaver.Application.Services.RealisationServices;

public class UserAccountService : IUserAccountService
{
    public UserAccountService(IPasswordHelper passwordHelper, IUserAccountRepository accountRepository, IUserRepository userRepository)
    {
        _passwordHelper = passwordHelper;
        _accountRepository = accountRepository;
        _userRepository = userRepository;
    }
    
    private readonly IPasswordHelper _passwordHelper;
    private readonly IUserAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    
    
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
}