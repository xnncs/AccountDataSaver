using AccountDataSaver.Api.Contracts;
using AccountDataSaver.Api.Models;
using AccountDataSaver.Api.Validation;
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
    
    public static async Task<IResult> DeleteById(int accountId, HttpContext context, IAuthorizationService authorizationService, 
        IJwtProvider jwtProvider, ILogger<UserAccountService> logger, IUserAccountService accountService)
    {
        // validation
        ValidationResultsValues validationResults = IsIdValid(accountId);
        if (!validationResults.IsValid)
        {
            return TypedResults.BadRequest(validationResults.GetModelValidationMistakes());
        }
        
        string userLogin = authorizationService.GetLoginFromJwt(context, jwtProvider);

        DeleteAccountRequestModel contract = DeleteAccountRequestModel.Create(
            accountId, userLogin);
        
        try
        {
            await accountService.DeleteAsync(contract);
            return TypedResults.Ok();
        }
        catch (Exception exception)
        {
            return HandleException(exception, userLogin, logger);
        }
    }

    public static async Task<IResult> UpdateById([FromBody] UpdateAccountRequest request,
        IMapper mapper, HttpContext context, IAuthorizationService authorizationService, IJwtProvider jwtProvider, 
        ILogger<UserAccountService> logger, IUserAccountService accountService)
    {
        // validation
        ValidationResultsValues validationResults = request.IsValid();
        if (!validationResults.IsValid)
        {
            return TypedResults.BadRequest(validationResults.GetModelValidationMistakes());
        }
        
        string userLogin = authorizationService.GetLoginFromJwt(context, jwtProvider);

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
            return HandleException(exception, userLogin, logger);
        }
    }
    
    public static async Task<IResult> GetAllAccounts(IUserAccountService accountService,
        IMapper mapper, HttpContext context, IJwtProvider jwtProvider, ILogger<UserAccountService> logger,
        IAuthorizationService authorizationService)
    {
        string userLogin = authorizationService.GetLoginFromJwt(context, jwtProvider);
        
        try
        {
            IEnumerable<UserAccountModel> accounts = accountService.GetAll(userLogin);
            return TypedResults.Ok(accounts);
        }
        catch (Exception exception)
        {
            return HandleException(exception, userLogin, logger);
        }
    }
    
    public static async Task<IResult> GetAccountsByUrl([FromQuery] string accountUrl, 
        IUserAccountService accountService, IMapper mapper, HttpContext context, 
        IJwtProvider jwtProvider, ILogger<UserAccountService> logger, IAuthorizationService authorizationService)
    {
        // validation
        ValidationResultsValues validationResults = IsServiceUrlStringValid(accountUrl);
        if (!validationResults.IsValid)
        {
            // returns all mistakes in model
            return TypedResults.BadRequest(validationResults.GetModelValidationMistakes());
        }

        
        string userLogin = authorizationService.GetLoginFromJwt(context, jwtProvider);

        try
        {
            IEnumerable<UserAccountModel> accounts = accountService.GetByUrl(userLogin, accountUrl);
            return TypedResults.Ok(accounts);
        }
        catch (Exception exception)
        {
            return HandleException(exception, userLogin, logger);
        }
    }

    public static async Task<IResult> AddAccount([FromBody] AddAccountRequest request, 
        IUserAccountService accountService, IMapper mapper, IPasswordHelper passwordHelper, 
        HttpContext context, IJwtProvider jwtProvider, IAuthorizationService authorizationService, 
        ILogger<UserAccountService> logger)
    {
        // validation
        ValidationResultsValues validationResults = request.IsValid();
        if (!validationResults.IsValid)
        {
            // returns all mistakes in model
            return TypedResults.BadRequest(validationResults.GetModelValidationMistakes());
        }
        
        AddAccountRequestModel contract = mapper.Map<AddAccountRequest, AddAccountRequestModel>(request);

        string userLogin = authorizationService.GetLoginFromJwt(context, jwtProvider);
        
        contract.Password = GetPassword(request, passwordHelper);
        contract.AuthorLogin = userLogin;
        
        try
        {
            await accountService.AddAsync(contract);
            return TypedResults.Ok();
        }
        catch (Exception exception)
        {
            return HandleException(exception, userLogin, logger);
        }
    }

    private static IResult HandleException(Exception exception, string userData,
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


    private static ValidationResultsValues IsIdValid(int id)
    {
        bool isValid = id.ToString().Length < 1_000_000_000 && id > 0;
        if (isValid)
        {
            return new ValidationResultsValues(true, null);
        }

        string errorMessage = "Id must be more then 0 and less then 1 000 000 000";
        return ValidationResultsValues.Create(false, errorMessage);
    }
    private static ValidationResultsValues IsServiceUrlStringValid(string url)
    {
        bool isValid = url.Length is > 4 and < 20;
        if (isValid)
        {
            return new ValidationResultsValues(true, null);
        }

        string errorMessage = "Url length must be more then 4 and less then 20";
        return ValidationResultsValues.Create(false, errorMessage);
    }
}