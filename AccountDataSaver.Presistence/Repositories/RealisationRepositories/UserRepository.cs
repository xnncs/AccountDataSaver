using AccountDataSaver.Core.Models;
using AccountDataSaver.Presistence.Entities;
using AccountDataSaver.Presistence.Repositories.AbstractRepositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccountDataSaver.Presistence.Repositories.RealisationRepositories;

public class UserRepository : IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext, IMapper mapper, ILogger<UserRepository> logger)
    {
        _logger = logger;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    private readonly ILogger<UserRepository> _logger;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _dbContext;
    
    public async Task AddAsync(UserModel user)
    {
        UserEntity userEntity = _mapper.Map<UserModel, UserEntity>(user);
        
        try
        {
            _dbContext.Users.Add(userEntity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
        }
    }

    public async Task<UserModel>? GetByLoginAsync(string login)
    {
        UserEntity? userEntity = await _dbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Login == login);
        return _mapper.Map<UserEntity, UserModel>(userEntity!);
    }

    public bool ContainsByLogin(string login)
    {
        return _dbContext.Users.AsNoTracking()
            .Any(x => x.Login == login);
    }
}