namespace AccountDataSaver.Api.Contracts;

public record AccountContractRequest
{
    public string ServiceUrl { get; set; }
    public string Description { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}