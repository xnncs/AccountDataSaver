namespace AccountDataSaver.Presistence.Entities;

public record UserAccountEntity : IEntity
{
    public int Id { get; set; }
    
    public string ServiceUrl { get; set; }
    public string Description { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    
    public UserEntity Author { get; set; }
    public int AuthorId { get; set; }
}