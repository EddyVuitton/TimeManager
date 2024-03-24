using TimeManager.Domain.Auth;
using TimeManager.Domain.Entities;
using TimeManager.Domain.Forms;
using TimeManager.Domain.Http;

namespace TimeManager.WebUI.Services.Account;

public interface IAccountService
{
    Task<HttpResultT<UserToken>> LoginAsync(LoginAccountForm form);
    Task<HttpResult> RegisterAsync(RegisterAccountForm form);
    Task<HttpResultT<UserAccount>> GetUserByEmailAsync(string email);
}