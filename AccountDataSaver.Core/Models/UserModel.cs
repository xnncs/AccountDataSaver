namespace AccountDataSaver.Core.Models;

public record UserModel
{
    public static UserModel Create(string login, string passwordHash, string name)
    {
        return new UserModel()
        {
            Login = login,
            PasswordHash = passwordHash,
            Name = name
        };
    }
    #region AuthProperties
    
    public string Login { get; set; }
    public string PasswordHash { get; set; }

    #endregion
    
    public string Name { get; set; }
}