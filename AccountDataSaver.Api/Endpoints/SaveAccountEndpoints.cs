using System.Security.Authentication;
using AccountDataSaver.Api.Contracts;
using AccountDataSaver.Application.Contracts;
using AccountDataSaver.Application.Models;
using AccountDataSaver.Application.Services.AbstractServices;
using AccountDataSaver.Application.Services.RealisationServices;
using AccountDataSaver.Core.Models;
using AccountDataSaver.Infrastructure.Abstract;
using AccountDataSaver.Infrastructure.Models;
using AutoMapper;

namespace AccountDataSaver.Api.Endpoints;

public static class SaveAccountEndpoints
{
    public static IEndpointRouteBuilder MapSaveAccountEndpoints(this IEndpointRouteBuilder app)
    {
        IEndpointRouteBuilder endpoints = app.MapGroup("api/userAccounts")
            .RequireAuthorization();

        // POST: api/userAccounts/add
        endpoints.MapPost("add", AddAccount); 
        
        // POST: api/userAccounts/get
        endpoints.MapPost("get", GetAccount);        
        
        // POST: api/userAccounts/getAll
        endpoints.MapPost("getAll", GetAllAccounts);
        
        return endpoints;
    }
    
    public static async Task<IResult> GetAllAccounts(IUserAccountService accountService,
        IMapper mapper, HttpContext context, IJwtProvider jwtProvider, ILogger<UserAccountService> logger,
        IUserService userService)
    {
        string authorLogin = userService.GetLoginFromJwt(context, jwtProvider);

        try
        {
            IEnumerable<UserAccountModel> accounts = accountService.GetAllAsync(authorLogin).Result;
            return TypedResults.Ok(accounts != null! ? accounts : Enumerable.Empty<UserAccountModel>());
        }
        catch (Exception exception)
        {
            logger.LogError(exception.Message);
            return TypedResults.Problem(exception.Message);
        }
    }
    
    public static async Task<IResult> GetAccount(GetAccountRequest request, IUserAccountService accountService,
        IMapper mapper, HttpContext context, IJwtProvider jwtProvider, ILogger<UserAccountService> logger,
        IUserService userService)
    {
        GetAccountRequestModel contract = mapper.Map<GetAccountRequest, GetAccountRequestModel>(request);

        string authorLogin = userService.GetLoginFromJwt(context, jwtProvider);

        try
        {
            UserAccountModel account = accountService.GetByServiceUrlAsync(authorLogin, contract.ServiceUrl)!.Result;
            return TypedResults.Ok(account);
        }
        catch (Exception exception)
        {
            logger.LogError(exception.Message);
            return TypedResults.Problem(exception.Message);
        }
    }

    public static async Task<IResult> AddAccount(AddAccountRequest request, IUserAccountService accountService,
        IMapper mapper, IPasswordHelper passwordHelper, HttpContext context, IJwtProvider jwtProvider,
        IUserService userService)
    {
        AddAccountRequestModel contract = mapper.Map<AddAccountRequest, AddAccountRequestModel>(request);

        contract.Password = GetPassword(request, passwordHelper);
        contract.AuthorLogin = userService.GetLoginFromJwt(context, jwtProvider);
        
        try
        {
            await accountService.AddAsync(contract);
        }
        catch (Exception exception)
        {
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