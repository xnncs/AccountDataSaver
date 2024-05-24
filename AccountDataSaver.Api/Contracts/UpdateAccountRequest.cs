using AccountDataSaver.Core.Models;

namespace AccountDataSaver.Api.Contracts;

public record UpdateAccountRequest
{
    public int AccountId { get; set; }
    public AccountContractRequest Data { get; set; }
}