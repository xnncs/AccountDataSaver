using System.IdentityModel.Tokens.Jwt;
using AccountDataSaver.Api.Contracts;
using AccountDataSaver.Application.Contracts;
using AccountDataSaver.Application.Models;
using AccountDataSaver.Application.Services.AbstractServices;
using AccountDataSaver.Infrastructure.Abstract;
using AccountDataSaver.Infrastructure.Models;
using AutoMapper;

namespace AccountDataSaver.Api.Endpoints;

public static class SaveAccountEndpoints
{
    public static IEndpointRouteBuilder MapSaveAccountEndpoints(this IEndpointRouteBuilder app)
    {
        IEndpointRouteBuilder endpoints = app.MapGroup("api/userAccounts");

        // POST: api/userAccounts/add
        endpoints.MapPost("add", AddAccount); 
        
        // POST: api/userAccounts/get
        endpoints.MapPost("get", GetAccount);        
        
        // POST: api/userAccounts/getAll
        endpoints.MapPost("getAll", GetAllAccounts);
        
        return endpoints;
    }
    
    public static async Task<IResult> GetAllAccounts(GetAllAccountsRequest request)
    {
        throw new NotImplementedException();
    }
    
    public static async Task<IResult> GetAccount()
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> AddAccount(AddAccountRequest request, IUserAccountService accountService,
        IMapper mapper, IPasswordHelper passwordHelper, HttpContext context)
    {
        AddAccountRequestModel contract = mapper.Map<AddAccountRequest, AddAccountRequestModel>(request);

        GenerateRandomPasswordOptions randomPasswordOptions = request.GenerateRandomPasswordOptions;

        // gets password value, if client choose to generate it randomly, generates it, and if no, it takes it from request 
        contract.Password = randomPasswordOptions.ToGenerateRandomPassword
            ? passwordHelper.GenerateRandomPassword(randomPasswordOptions)
            : request.Password;
        
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

    private static string? getAuthorLogin(HttpContext context)
    {
        string? cookieJwtToken = context.Request.Cookies["tasty-cookies"];
        
        
    }
}