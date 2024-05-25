using AccountDataSaver.Api.Contracts;
using AccountDataSaver.Api.Models;
using AccountDataSaver.Api.Validation;
using AccountDataSaver.Application.Contracts;
using AccountDataSaver.Application.Services.AbstractServices;
using AutoMapper;
using RegisterUserRequest = AccountDataSaver.Api.Contracts.RegisterUserRequest;

namespace AccountDataSaver.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        IEndpointRouteBuilder endpoints = app.MapGroup("api/auth");
        
        // POST: api/auth/register
        endpoints.MapPost("register", RegisterAsync);
        
        // POST: api/auth/login
        endpoints.MapPost("login", LoginAsync);

        return endpoints;
    }

    public static async Task<IResult> RegisterAsync(RegisterUserRequest request, IAuthorizationService authorizationService, IMapper mapper)
    {
        ValidationResultsValues validationResults = request.IsValid();
        if (!validationResults.IsValid)
        {
            return TypedResults.BadRequest(validationResults.GetModelValidationMistakes());
        }
        
        RegisterUserRequestModel contract = mapper.Map<RegisterUserRequest, RegisterUserRequestModel>(request);
        try
        {
            await authorizationService.RegisterAsync(contract);
        }
        catch (Exception exception)
        {
            return TypedResults.Problem(exception.Message);
        }
        
        return TypedResults.Ok();
    }

    public static async Task<IResult> LoginAsync(LoginUserRequest request, IAuthorizationService authorizationService, HttpContext context, IMapper mapper)
    {
        ValidationResultsValues validationResults = request.IsValid();
        if (!validationResults.IsValid)
        {
            return TypedResults.BadRequest(validationResults.GetModelValidationMistakes());
        }
        
        LoginUserRequestModel contract = mapper.Map<LoginUserRequest, LoginUserRequestModel>(request);
        
        string token = await authorizationService.LoginAsync(contract);
        context.Response.Cookies.Append("tasty-cookies", token);
        
        return TypedResults.Ok();
    }
}

