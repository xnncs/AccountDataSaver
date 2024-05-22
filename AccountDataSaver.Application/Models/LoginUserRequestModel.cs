namespace AccountDataSaver.Application.Contracts;

public record LoginUserRequestModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}