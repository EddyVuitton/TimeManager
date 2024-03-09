using TimeManager.Domain.Auth;
using TimeManager.Domain.Forms;

namespace TimeManager.WebAPI.Repositories.Account;

public interface IAccount
{
    Task<UserToken> LoginAsync(LoginAccountForm form);
}