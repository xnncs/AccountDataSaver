namespace AccountDataSaver.Api.Contracts;

public record GetAccountRequest
{
    public string ServiceUrl { get; set; }
}