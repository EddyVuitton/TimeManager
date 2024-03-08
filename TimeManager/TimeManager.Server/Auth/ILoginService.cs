using TimeManager.Domain.Auth;

namespace TimeManager.WebAPI.Auth;

public interface ILoginService
{
    Task LoginAsync(UserToken userToken);
    Task LogoutAsync();
    Task<int> IsLoggedInAsync();
    Task LogoutIfExpiredTokenAsync();
}