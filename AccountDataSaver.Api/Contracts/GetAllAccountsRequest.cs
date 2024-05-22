using AccountDataSaver.Core.Models;

namespace AccountDataSaver.Api.Contracts;

public record GetAllAccountsRequest(UserModel RequestingUser);