using Microsoft.IdentityModel.Tokens;
using System.Text;
using TimeManager.Domain.Auth;
using TimeManager.WebAPI.Helpers;

namespace TimeManager.WebAPI.Repositories.Account;

public class Account : IAccount
{
    private readonly byte[] _jwtKeyBytes = Encoding.UTF8.GetBytes(ConfigurationHelper.JWTKey);

    public async Task<UserToken> LoginAsync(string email, string password)
    {
        var hashedPassword = AuthHelper.HashPassword(password);
        var dbAccountPassword = AuthHelper.HashPassword("admin");

        if (dbAccountPassword == hashedPassword)
        {
            var key = new SymmetricSecurityKey(_jwtKeyBytes);
            var token = AuthHelper.BuildToken(email, key);

            return token;
        }
        else
        {
            throw new Exception("Nieprawidłowe hasło lub email");
        }
    }
}