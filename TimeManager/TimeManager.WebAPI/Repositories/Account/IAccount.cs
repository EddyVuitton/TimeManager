using TimeManager.Domain.Auth;
using TimeManager.Domain.Entities;
using TimeManager.Domain.Forms;

namespace TimeManager.WebAPI.Repositories.Account;

public interface IAccount
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<UserToken> LoginAsync(LoginAccountForm form);
    Task RegisterAsync(RegisterAccountForm form);
}