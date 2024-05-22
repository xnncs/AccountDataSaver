namespace AccountDataSaver.Api.Contracts;

public record RegisterUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}