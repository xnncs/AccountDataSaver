using AccountDataSaver.Api.Contracts;
using AccountDataSaver.Application.Contracts;
using AccountDataSaver.Application.Models;
using AccountDataSaver.Application.Services.AbstractServices;
using AccountDataSaver.Application.Services.RealisationServices;
using AccountDataSaver.Core.Models;
using AccountDataSaver.Infrastructure.Abstract;
using AccountDataSaver.Infrastructure.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AccountDataSaver.Api.Endpoints;

public static class SaveAccountEndpoints
{
    public static IEndpointRouteBuilder MapSaveAccountEndpoints(this IEndpointRouteBuilder app)
    {
        IEndpointRouteBuilder endpoints = app.MapGroup("api/userAccounts")
            .RequireAuthorization();

        
        // POST: api/userAccounts/add
        endpoints.MapPost("add", AddAccount); 
        
        // POST: api/userAccounts/getAccountsByUrl
        endpoints.MapGet("getAccountsById", GetAccountsByUrl);        
        
        // POST: api/userAccounts/getAll
        endpoints.MapGet("getAll", GetAllAccounts);

        // POST: api/userAccounts/updateById
        endpoints.MapPut("updateById", UpdateById);     
        

        return endpoints;
    }

    public static async Task<IResult> UpdateById([FromBody] UpdateAccountRequest request,
        IMapper mapper, HttpContext context, IUserService userService, IJwtProvider jwtProvider, 
        ILogger<UserAccountService> logger, IUserAccountService accountService)
    {
        string userLogin = userService.GetLoginFromJwt(context, jwtProvider);

        UpdateAccountRequestModel contract = new UpdateAccountRequestModel()
        {
            Data = mapper.Map<AccountContractRequest, UserAccountModel>(request.Data),
            AccountId = request.AccountId,
            RequestUserLogin = userLogin
        };

        try
        {
            await accountService.UpdateByIdAsync(contract);
            return TypedResults.Ok();
        }
        catch (Exception exception)
        {
            logger.LogError($"user with login {userLogin} had exception {exception.Message}");
            return TypedResults.Problem(exception.Message);
        }
    }
    
    public static async Task<IResult> GetAllAccounts(IUserAccountService accountService,
        IMapper mapper, HttpContext context, IJwtProvider jwtProvider, ILogger<UserAccountService> logger,
        IUserService userService)
    {
        string userLogin = userService.GetLoginFromJwt(context, jwtProvider);
        
        try
        {
            IEnumerable<UserAccountModel> accounts = accountService.GetAllAsync(userLogin).Result;
            // if collection is null, returns empty collection
            return TypedResults.Ok(accounts != null! ? accounts : Enumerable.Empty<UserAccountModel>());
        }
        catch (Exception exception)
        {
            logger.LogError($"user with login {userLogin} had exception {exception.Message}");
            return TypedResults.Problem(exception.Message);
        }
    }
    
    public static async Task<IResult> GetAccountsByUrl([FromBody] GetAccountRequest request, 
        IUserAccountService accountService, IMapper mapper, HttpContext context, 
        IJwtProvider jwtProvider, ILogger<UserAccountService> logger, IUserService userService)
    {
        GetAccountRequestModel contract = mapper.Map<GetAccountRequest, GetAccountRequestModel>(request);

        string userLogin = userService.GetLoginFromJwt(context, jwtProvider);

        try
        {
            IEnumerable<UserAccountModel> accounts = accountService.GetAllAsync(userLogin).Result
                .Where(x => x.ServiceUrl == contract.ServiceUrl);
            // if collection is null, returns empty collection
            return TypedResults.Ok(accounts != null! ? accounts : Enumerable.Empty<UserAccountModel>());
        }
        catch (Exception exception)
        {
            logger.LogError($"user with login {userLogin} had exception {exception.Message}");
            return TypedResults.Problem(exception.Message);
        }
    }

    public static async Task<IResult> AddAccount([FromBody] AddAccountRequest request, 
        IUserAccountService accountService, IMapper mapper, IPasswordHelper passwordHelper, 
        HttpContext context, IJwtProvider jwtProvider, IUserService userService, 
        ILogger<UserAccountService> logger)
    {
        AddAccountRequestModel contract = mapper.Map<AddAccountRequest, AddAccountRequestModel>(request);

        string userLogin = userService.GetLoginFromJwt(context, jwtProvider);
        
        contract.Password = GetPassword(request, passwordHelper);
        contract.AuthorLogin = userLogin;
        
        try
        {
            await accountService.AddAsync(contract);
        }
        catch (Exception exception)
        {
            logger.LogError($"user with login {userLogin} had exception {exception.Message}");
            return TypedResults.Problem(exception.Message);
        }

        return TypedResults.Ok();
    }

    // gets password value, if client choose to generate it randomly, generates it, and if no, it takes it from request 
    private static string GetPassword(AddAccountRequest request, IPasswordHelper passwordHelper)
    {
        GenerateRandomPasswordOptions randomPasswordOptions = request.GenerateRandomPasswordOptions;
        
        return randomPasswordOptions.ToGenerateRandomPassword
            ? passwordHelper.GenerateRandomPassword(randomPasswordOptions)
            : request.Password;
    }
}