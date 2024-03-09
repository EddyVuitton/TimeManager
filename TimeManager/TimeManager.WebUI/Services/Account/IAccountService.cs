using TimeManager.Domain.Auth;
using TimeManager.Domain.Forms;
using TimeManager.Domain.Http;

namespace TimeManager.WebUI.Services.Account;

public interface IAccountService
{
    Task<HttpResultT<UserToken>> LoginAsync(LoginAccountForm form);
}