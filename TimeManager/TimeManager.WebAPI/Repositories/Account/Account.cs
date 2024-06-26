﻿using Microsoft.EntityFrameworkCore;
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
    private readonly DBContext _context = context;

    private readonly byte[] _jwtKeyBytes = Encoding.UTF8.GetBytes(ConfigurationHelper.JWTKey);

    public async Task<UserToken> LoginAsync(LoginAccountForm form)
    {
        if (form is null || form.Email is null || form.Password is null)
            throw new Exception("Niepoprawna próba logowania");

        var hashedPassword = AuthHelper.HashPassword(form.Password);
        var dbAccountPassword = (await _context.UserAccount.FirstOrDefaultAsync(x => x.Email == form.Email))?.Password;

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

        var doesExist = await _context.UserAccount.FirstOrDefaultAsync(x => x.Email == form.Email);

        if (doesExist is not null)
            throw new Exception("Ten adres email jest zajęty");

        var user = new UserAccount()
        {
            Email = form.Email,
            Password = AuthHelper.HashPassword(form.Password)
        };

        var activityList = new ActivityList()
        {
            Name = "Moje zadania",
            UserAccount = user
        };

        await _context.UserAccount.AddAsync(user);
        await _context.ActivityList.AddAsync(activityList);

        await _context.SaveChangesAsync();
    }

    public async Task<UserAccount?> GetUserByEmailAsync(string email)
    {
        var user = await _context.UserAccount.FirstOrDefaultAsync(x => x.Email == email);
        return user;
    }
}