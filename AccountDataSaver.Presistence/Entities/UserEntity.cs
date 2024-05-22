namespace AccountDataSaver.Presistence.Entities;

public record UserEntity : IEntity
{
    #region AuthProperties
    
    public int Id { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }

    #endregion
    
    public string Name { get; set; }

    public ICollection<UserAccountEntity> Accounts { get; set; } = Enumerable.Empty<UserAccountEntity>().ToList();
}