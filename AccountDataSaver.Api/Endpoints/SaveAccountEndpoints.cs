using AccountDataSaver.Api.Contracts;
using AccountDataSaver.Application.Models;
using AccountDataSaver.Application.Services.AbstractServices;
using AccountDataSaver.Application.Services.RealisationServices;
using AccountDataSaver.Core.Models;
using AccountDataSaver.Infrastructure.Abstract;
using AccountDataSaver.Infrastructure.Models;
using AutoMapper;
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
        
        // POST: api/userAccounts/getByUrl
        endpoints.MapGet("getByUrl", GetAccountsByUrl);        
        
        // POST: api/userAccounts/get
        endpoints.MapGet("get", GetAllAccounts);

        // POST: api/userAccounts/update
        endpoints.MapPut("update", UpdateById);     
        
        // DELETE: api/userAccounts/delete
        endpoints.MapDelete("delete", DeleteById);
        

        return endpoints;
    }

    public static async Task<IResult> DeleteById(int accountId, HttpContext context, IUserService userService, 
        IJwtProvider jwtProvider, ILogger<UserAccountService> logger, IUserAccountService accountService)
    {
        string userLogin = userService.GetLoginFromJwt(context, jwtProvider);

        DeleteAccountRequestModel contract = DeleteAccountRequestModel.Create(
            accountId, userLogin);

        
        try
        {
            await accountService.DeleteAsync(contract);
            return TypedResults.Ok();
        }
        catch (Exception exception)
        {
            return await HandleException(exception, userLogin, logger);
        }
    }

    public static async Task<IResult> UpdateById([FromBody] UpdateAccountRequest request,
        IMapper mapper, HttpContext context, IUserService userService, IJwtProvider jwtProvider, 
        ILogger<UserAccountService> logger, IUserAccountService accountService)
    {
        string userLogin = userService.GetLoginFromJwt(context, jwtProvider);

        UpdateAccountRequestModel contract = UpdateAccountRequestModel.Create(
            request.AccountId,
            mapper.Map<AccountContractRequest, UserAccountModel>(request.Data),
            userLogin);


        try
        {
            await accountService.UpdateAsync(contract);
            return TypedResults.Ok();
        }
        catch (Exception exception)
        {
            return await HandleException(exception, userLogin, logger);
        }
    }
    
    public static async Task<IResult> GetAllAccounts(IUserAccountService accountService,
        IMapper mapper, HttpContext context, IJwtProvider jwtProvider, ILogger<UserAccountService> logger,
        IUserService userService)
    {
        string userLogin = userService.GetLoginFromJwt(context, jwtProvider);
        
        try
        {
            IEnumerable<UserAccountModel> accounts = accountService.GetAll(userLogin);
            // if collection is null, returns empty collection
            return TypedResults.Ok(accounts != null! ? accounts : Enumerable.Empty<UserAccountModel>());
        }
        catch (Exception exception)
        {
            return await HandleException(exception, userLogin, logger);
        }
    }
    
    public static async Task<IResult> GetAccountsByUrl([FromQuery] string accountUrl, 
        IUserAccountService accountService, IMapper mapper, HttpContext context, 
        IJwtProvider jwtProvider, ILogger<UserAccountService> logger, IUserService userService)
    {
        string userLogin = userService.GetLoginFromJwt(context, jwtProvider);

        try
        {
            IEnumerable<UserAccountModel> accounts = accountService.GetAll(userLogin).AsEnumerable()
                .Where(x => x.ServiceUrl == accountUrl);
            // if collection is null, returns empty collection
            return TypedResults.Ok(accounts != null! ? accounts : Enumerable.Empty<UserAccountModel>());
        }
        catch (Exception exception)
        {
            return await HandleException(exception, userLogin, logger);
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
            return TypedResults.Ok();
        }
        catch (Exception exception)
        {
            return await HandleException(exception, userLogin, logger);
        }

    }

    private static async Task<IResult> HandleException(Exception exception, string userData,
        ILogger<UserAccountService> logger)
    {
        logger.LogError($"user with login {userData} had exception {exception.Message}");
        return TypedResults.Problem(exception.Message);
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