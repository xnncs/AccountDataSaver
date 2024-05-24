using AccountDataSaver.Core.Models;

namespace AccountDataSaver.Application.Models;

public record UpdateAccountRequestModel
{
    public static UpdateAccountRequestModel Create(int accountId, UserAccountModel data, string requestUserLogin)
    {
        return new UpdateAccountRequestModel()
        {
            AccountId = accountId,
            Data = data,
            RequestUserLogin = requestUserLogin
        };
    }
    
    public int AccountId { get; set; }
    public UserAccountModel Data { get; set; }
    
    public string RequestUserLogin { get; set; }
}