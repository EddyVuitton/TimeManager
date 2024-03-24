using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TimeManager.Domain.Auth;
using TimeManager.Domain.Context;
using TimeManager.Domain.Entities;
using TimeManager.Domain.Forms;
using TimeManager.WebAPI.Helpers;

namespace TimeManager.WebAPI.Repositories.Account;

public class Account(DBContext context) : IAccount
{
    private readonly byte[] _jwtKeyBytes = Encoding.UTF8.GetBytes(ConfigurationHelper.JWTKey);

    public async Task<UserToken> LoginAsync(LoginAccountForm form)
    {
        if (form is null || form.Email is null || form.Password is null)
            throw new Exception("Niepoprawna próba logowania");

        var hashedPassword = AuthHelper.HashPassword(form.Password);
        var dbAccountPassword = (await context.User.FirstOrDefaultAsync(x => x.Email == form.Email))?.Password;

        if (dbAccountPassword == hashedPassword)
        {
            var key = new SymmetricSecurityKey(_jwtKeyBytes);
            var token = AuthHelper.BuildToken(form.Email, key);

            return token;
        }
        else
        {
            throw new Exception("Nieprawidłowe hasło lub email");
        }
    }

    public async Task RegisterAsync(RegisterAccountForm form)
    {
        if (form is null || form.Email is null || form.Password is null)
            throw new Exception("Niepoprawna próba rejestracji");

        var doesExist = await context.User.FirstOrDefaultAsync(x => x.Email == form.Email);

        if (doesExist is not null)
            throw new Exception("Ten adres email jest zajęty");

        var user = new User()
        {
            Email = form.Email,
            Password = AuthHelper.HashPassword(form.Password)
        };

        var activityList = new ActivityList()
        {
            Name = "Moje zadania",
            User = user
        };

        await context.User.AddAsync(user);
        await context.ActivityList.AddAsync(activityList);

        await context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var user = await context.User.FirstOrDefaultAsync(x => x.Email == email);
        return user;
    }
}