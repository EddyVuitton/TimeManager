using TimeManager.Domain.Auth;
using TimeManager.Domain.Http;

namespace TimeManager.WebUI.Services.AccontService;

public interface IAccountService
{
    Task<HttpResultT<UserToken>> LoginAsync(string email, string password);
}