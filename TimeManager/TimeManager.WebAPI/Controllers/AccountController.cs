using Microsoft.AspNetCore.Mvc;
using TimeManager.Domain.Auth;
using TimeManager.Domain.Http;
using TimeManager.WebAPI.Helpers;
using TimeManager.WebAPI.Repositories.Account;

namespace TimeManager.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccount businessLogic) : ControllerBase
{
    private readonly IAccount _businessLogic = businessLogic;

    #region Posts

    [HttpPost("LoginAsync")]
    public async Task<HttpResultT<UserToken>> LoginAsync(string email, string password)
    {
        try
        {
            var result = await _businessLogic.LoginAsync(email, password);
            return HttpHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpHelper.Error<UserToken>(e);
        }
    }

    #endregion Posts
}