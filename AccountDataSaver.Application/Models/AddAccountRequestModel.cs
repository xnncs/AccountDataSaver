namespace AccountDataSaver.Application.Models;

public record AddAccountRequestModel
{
    // public AddAccountRequestModel(string authorLogin,
    //     string serviceUrl,
    //     string description,
    //     string login,
    //     string password
    //     )
    // {
    //     AuthorLogin = authorLogin;
    //     ServiceUrl = serviceUrl;
    //     Description = description;
    //     Login = login;
    //     Password = password;
    // }
    
    public string AuthorLogin { get; set; }
    public string ServiceUrl { get; set; } 
    public string Description { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}