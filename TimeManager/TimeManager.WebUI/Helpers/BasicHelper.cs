using TimeManager.Domain.DTOs;

namespace TimeManager.WebUI.Helpers;

public static class BasicHelper
{
    public static string GetMonthName(int month)
    {
        return month switch
        {
            1 => "Styczeń",
            2 => "Luty",
            3 => "Marzec",
            4 => "Kwiecień",
            5 => "Maj",
            6 => "Czerwiec",
            7 => "Lipiec",
            8 => "Sierpień",
            9 => "Wrzesień",
            10 => "Październik",
            11 => "Listopad",
            12 => "Grudzień",
            _ => string.Empty,
        };
    }

    public static string GetDayWeekName(int day)
    {
        return day switch
        {
            0 => "Niedziela",
            1 => "Poniedziałek",
            2 => "Wtorek",
            3 => "Środa",
            4 => "Czwartek",
            5 => "Piątek",
            6 => "Sobota",
            _ => string.Empty,
        };
    }

    public static string GetPolishMonthInflection(int month)
    {
        return month switch
        {
            1 => "Stycznia",
            2 => "Lutego",
            3 => "Marca",
            4 => "Kwietnia",
            5 => "Maja",
            6 => "Czerwca",
            7 => "Lipca",
            8 => "Sierpnia",
            9 => "Września",
            10 => "Października",
            11 => "Listopada",
            12 => "Grudnia",
            _ => string.Empty,
        };
    }

    public static MonthDto CreateMonthDto(int year, int month, List<ActivityDto>? activities)
    {
        var monthDto = new MonthDto
        {
            Year = year,
            Month = month
        };

        var daysInMonth = new DateTime(year, month, 1).AddMonths(1).AddDays(-1).Day;

        for (int i = 1; i <= daysInMonth; i++)
        {
            var day = new DateTime(year, month, i);
            var monthActivities = GetMonthActivities(activities, day);

            var dayDto = new DayDto()
            {
                Day = day,
                Activities = monthActivities.Where(x => x.Day.Day == i).ToList()
            };
            monthDto.Days.Add(dayDto);
        }

        return monthDto;
    }

    public static List<ActivityDto> GetMonthActivities(List<ActivityDto>? activities, DateTime date) =>
        activities?.Where(x => x.Day.Year == date.Year && x.Day.Month == date.Month).ToList() ?? new List<ActivityDto>();
}