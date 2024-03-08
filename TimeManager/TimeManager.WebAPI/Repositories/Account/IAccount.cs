using TimeManager.Domain.Auth;

namespace TimeManager.WebAPI.Repositories.Account;

public interface IAccount
{
    Task<UserToken> LoginAsync(string email, string password);
}