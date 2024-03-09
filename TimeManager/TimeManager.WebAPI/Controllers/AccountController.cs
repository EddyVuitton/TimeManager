using Microsoft.AspNetCore.Mvc;
using TimeManager.Domain.Auth;
using TimeManager.Domain.Forms;
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
    public async Task<HttpResultT<UserToken>> LoginAsync(LoginAccountForm form)
    {
        try
        {
            var result = await _businessLogic.LoginAsync(form);
            return HttpHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpHelper.Error<UserToken>(e);
        }
    }

    #endregion Posts
}