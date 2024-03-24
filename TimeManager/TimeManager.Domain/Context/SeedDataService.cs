using TimeManager.Domain.Entities;

namespace TimeManager.Domain.Context;

public static class SeedDataService
{
    public static void Initialize(DBContext context)
    {
        #region Dictionaries

        //RepetitionType
        AddRepetitionTypes(context);
        context.SaveChanges();

        //HourType
        AddHourTypes(context);
        context.SaveChanges();

        #endregion Dictionaries

        //UserAccount
        var user = new UserAccount() { Email = "konto@demo.com", Password = "2A97516C354B68848CDBD8F54A226A0A55B21ED138E207AD6C5CBB9C00AA5AEA" }; //demo
        context.UserAccount.Add(user);
        context.SaveChanges();

        //Repetition
        var repetition1 = AddRepetition(context, 1);

        //Default hour type
        var hourTypeId = context.HourType.First(x => x.Name == "10:00").Id;

        //Default activity list
        var activityList = new ActivityList()
        {
            Name = "Moje zadania",
            User = user
        };
        context.ActivityList.Add(activityList);

        //Activity
        var activity1 = new Activity()
        {
            Day = DateTime.Now,
            Description = string.Empty,
            Task = "Moje zadania",
            HourTypeId = hourTypeId,
            Repetition = repetition1,
            User = user,
            ActivityList = activityList
        };

        context.Activity.Add(activity1);

        context.SaveChanges();
    }

    private static void AddRepetitionTypes(DBContext context)
    {
        context.RepetitionType.Add(new RepetitionType() { Name = "Nie powtarza się" });
        context.RepetitionType.Add(new RepetitionType() { Name = "Codziennie" });
        context.RepetitionType.Add(new RepetitionType() { Name = "Co tydzień" });
        context.RepetitionType.Add(new RepetitionType() { Name = "Co miesiąc" });
        context.RepetitionType.Add(new RepetitionType() { Name = "Co roku" });
        context.RepetitionType.Add(new RepetitionType() { Name = "W dni powszednie (od poniedziałku do piątku)" });
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

    private static void AddHourTypes(DBContext context)
    {
        var now = DateTime.Now;
        var start = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

        for (int i = 0; i < 96; i++)
        {
            var hour = start.Hour < 10 ? $"0{start.Hour}" : start.Hour.ToString();
            var minute = start.Minute < 10 ? $"0{start.Minute}" : start.Minute.ToString();
            var hourType = new HourType() { Name = $"{hour}:{minute}" };

            context.HourType.Add(hourType);
            start = start.AddMinutes(15);
        }
    }
}