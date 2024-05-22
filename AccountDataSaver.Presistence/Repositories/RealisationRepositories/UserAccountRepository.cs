using AccountDataSaver.Core.Models;
using AccountDataSaver.Presistence.Entities;
using AccountDataSaver.Presistence.Repositories.AbstractRepositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccountDataSaver.Presistence.Repositories.RealisationRepositories;

public class UserAccountRepository : IUserAccountRepository
{
    public UserAccountRepository(ApplicationDbContext dbContext, IMapper mapper, ILogger<UserAccountRepository> logger)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _logger = logger;
    }

    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UserAccountRepository> _logger;
    
    public async Task AddAsync(UserAccountModel account)
    {
        UserAccountEntity accountEntity = _mapper.Map<UserAccountModel, UserAccountEntity>(account);

        UserEntity author = _dbContext.Users.FirstOrDefault(x => x.Login == account.AuthorLogin)!;
        
        accountEntity.Author = author;
        accountEntity.AuthorId = author.Id;
        
        author.Accounts.Add(accountEntity);
        
        try
        {
            _dbContext.UserAccounts.Add(accountEntity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
        }
    }

    public bool Contains(string serviceUrl, string login, string password)
    {
        return _dbContext.UserAccounts.AsNoTracking()
            .Any(
            x =>
                x.Login == login &&
                x.Password == password &&
                x.ServiceUrl == serviceUrl);
    }

    public IQueryable<UserAccountModel> GetAllAccounts(string authorLogin)
    {
        UserEntity author = _dbContext.Users.FirstOrDefault(x => x.Login == authorLogin)
                            ?? throw new Exception("No authors with such login");

        IQueryable<UserAccountEntity> accountsEntities = _dbContext.UserAccounts.Where(x => x.AuthorId == author.Id);

        // mapping every element of accountsEntities from entity to model
        return accountsEntities.Select(x => _mapper.Map<UserAccountEntity, UserAccountModel>(x));
    }

    public UserAccountModel GetAccountByUrl(string authorLogin, string serviceUrl)
    {
        UserEntity author = _dbContext.Users.FirstOrDefault(x => x.Login == authorLogin)
                            ?? throw new Exception("no such authors with such login");

        IQueryable<UserAccountEntity> accountsEntities = _dbContext.UserAccounts.Where(x => x.AuthorId == author.Id);
        UserAccountEntity account = accountsEntities.FirstOrDefault(x => x.ServiceUrl == serviceUrl)
                                    ?? throw new Exception("that author does not have any services with that url");

        return _mapper.Map<UserAccountEntity, UserAccountModel>(account);
    }
}