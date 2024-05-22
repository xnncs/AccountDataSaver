using AccountDataSaver.Api.Contracts;
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

    public static async Task<IResult> RegisterAsync(RegisterUserRequest request, IUserService userService, IMapper mapper)
    {
        RegisterUserRequestModel contract = mapper.Map<RegisterUserRequest, RegisterUserRequestModel>(request);
        try
        {
            await userService.RegisterAsync(contract);
        }
        catch (Exception exception)
        {
            return TypedResults.Problem(exception.Message);
        }
        
        return TypedResults.Ok();
    }

    public static async Task<IResult> LoginAsync(LoginUserRequest request, IUserService userService, HttpContext context, IMapper mapper)
    {
        LoginUserRequestModel contract = mapper.Map<LoginUserRequest, LoginUserRequestModel>(request);
        
        string token = await userService.LoginAsync(contract);
        context.Response.Cookies.Append("tasty-cookies", token);
        
        return TypedResults.Ok();
    }
}

