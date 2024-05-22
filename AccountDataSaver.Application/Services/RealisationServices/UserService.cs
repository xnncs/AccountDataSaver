using AccountDataSaver.Application.Contracts;
using AccountDataSaver.Application.Services.AbstractServices;
using AccountDataSaver.Core.Models;
using AccountDataSaver.Infrastructure.Abstract;
using AccountDataSaver.Presistence.Repositories.AbstractRepositories;
using Microsoft.AspNetCore.Identity;

namespace AccountDataSaver.Application.Services.RealisationServices;

public class UserService : IUserService
{
    public UserService(IPasswordHasher passwordHasher, IUserRepository userRepository, IJwtProvider jwtProvider)
    {
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }

    private readonly IJwtProvider _jwtProvider;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    
    public async Task RegisterAsync(RegisterUserRequestModel request)
    {
        if (_userRepository.ContainsByLogin(request.Email))
        {
            throw new Exception("Request login is already used");
        }
        
        string passwordHash = _passwordHasher.HashPassword(request.Password);
        UserModel user = UserModel.Create(request.Email, passwordHash, request.Name);
            
        await _userRepository.AddAsync(user);
    }

    public async Task<string> LoginAsync(LoginUserRequestModel request)
    {
        UserModel userModel = await _userRepository.GetByLoginAsync(request.Email)!
                              ?? throw new Exception("Failed to login, no users with such email");

        PasswordVerificationResult state = _passwordHasher.VerifyHashedPassword(
            providedPassword: request.Password,
            hashedPassword: userModel.PasswordHash);
        
        if (state == PasswordVerificationResult.Failed)
        {
            throw new Exception("Failed to login, incorrect password");
        }
            
        string token = _jwtProvider.GenerateToken(userModel);
        return token;
    }
}