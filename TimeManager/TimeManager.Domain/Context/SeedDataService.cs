﻿using TimeManager.Domain.Entities;

namespace TimeManager.Domain.Context;

public static class SeedDataService
{
    public static void Initialize(DBContext context)
    {
        //RepetitionType
        context.RepetitionType.Add(new RepetitionType() { Name = "Nie powtarza się" });
        context.RepetitionType.Add(new RepetitionType() { Name = "Codziennie" });
        context.RepetitionType.Add(new RepetitionType() { Name = "Co tydzień" });
        context.RepetitionType.Add(new RepetitionType() { Name = "Co miesiąc" });
        context.RepetitionType.Add(new RepetitionType() { Name = "Co roku" });
        context.RepetitionType.Add(new RepetitionType() { Name = "W dni powszednie (od poniedziałku do piątku)" });

        //User
        context.User.Add(new User() { Email = "abc@ab.com", Password = "admin" });

        //Repetition
        context.Repetition.Add(new Repetition() {  RepetitionTypeId = 1 });
        context.Repetition.Add(new Repetition() {  RepetitionTypeId = 1 });

        //Activity
        context.Activity.Add(new Activity() { 
            Day = DateTime.Now,
            Task="Moje zadania",
            Hour="10:00",
            RepetitionId = 1,
            UserId = 1
        });
        context.Activity.Add(new Activity()
        {
            Day = DateTime.Now.AddDays(1),
            Task="Moje zadania",
            Hour="12:00",
            RepetitionId = 2,
            UserId = 1
        });

        //zapisz zmiany w bazie danych
        context.SaveChanges();
    }
}