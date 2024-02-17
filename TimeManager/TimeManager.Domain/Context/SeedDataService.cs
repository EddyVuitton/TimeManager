using TimeManager.Domain.Entities;

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
        context.SaveChanges();

        //User
        var user = new User() { Email = "abc@ab.com", Password = "admin" };
        context.User.Add(user);
        context.SaveChanges();

        //Repetition
        var repetition1 = AddRepetition(context, 1);
        var repetition2 = AddRepetition(context, 1);
        var repetition3 = AddRepetition(context, 1);

        //Activity
        var activity1 = new Activity()
        {
            Day = DateTime.Now,
            Description = string.Empty,
            Task = "Moje zadania",
            Hour = "10:00",
            Repetition = repetition1,
            User = user
        };
        var activity2 = new Activity()
        {
            Day = activity1.Day.AddDays(1),
            Description = string.Empty,
            Task = "Moje zadania",
            Hour = "10:00",
            Repetition = repetition2,
            User = user
        };
        var activity3 = new Activity()
        {
            Day = activity2.Day.AddDays(1),
            Description = string.Empty,
            Task = "Moje zadania",
            Hour = "10:00",
            Repetition = repetition3,
            User = user
        };

        context.Activity.Add(activity1);
        context.Activity.Add(activity2);
        context.Activity.Add(activity3);

        context.SaveChanges();
    }

    private static Repetition AddRepetition(DBContext context, int typeId)
    {
        var repetition = new Repetition()
        {
            RepetitionTypeId = typeId
        };
        context.Repetition.Add(repetition);
        context.SaveChanges();
        return repetition;
    }
}