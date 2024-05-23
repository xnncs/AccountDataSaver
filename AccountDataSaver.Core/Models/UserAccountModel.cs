namespace AccountDataSaver.Core.Models;

public record UserAccountModel
{
    public static UserAccountModel Create(string serviceUrl, string description, string login, string password, string authorLogin)
    {
        return new UserAccountModel()
        {
            ServiceUrl = serviceUrl,
            Description = description,
            Login = login,
            Password = password,
            AuthorLogin = authorLogin
        };
    }
    
    public int Id { get; set; }
    public string ServiceUrl { get; set; }
    public string Description { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    
    public string AuthorLogin { get; set; }
}