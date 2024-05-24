namespace AccountDataSaver.Application.Models;

public record DeleteAccountRequestModel
{
    public static DeleteAccountRequestModel Create(int accountId, string requestUserLogin)
    {
        return new DeleteAccountRequestModel()
        {
            AccountId = accountId,
            RequestUserLogin = requestUserLogin
        };
    }
    public int AccountId { get; set; }
    public string RequestUserLogin { get; set; }
}