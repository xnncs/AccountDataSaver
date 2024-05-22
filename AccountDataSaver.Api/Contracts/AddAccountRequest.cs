using AccountDataSaver.Application.Contracts;
using AccountDataSaver.Infrastructure.Models;

namespace AccountDataSaver.Api.Contracts;

public record AddAccountRequest
{
    // public AddAccountRequest(string authorLogin, string serviceUrl, string description,
    //     string login, GenerateRandomPasswordOptions generateRandomPasswordOptions, string password)
    // {
    //     AuthorLogin = authorLogin;
    //     ServiceUrl = serviceUrl;
    //     Description = description;
    //     Login = login;
    //     GenerateRandomPasswordOptions = generateRandomPasswordOptions;
    //     Password = password;
    // }
    
    public string AuthorLogin { get; set; }
    public string ServiceUrl { get; set; } 
    public string Description { get; set; }
    public string Login { get; set; }
    
    public GenerateRandomPasswordOptions GenerateRandomPasswordOptions { get; set; }
    public string Password { get; set; }
}