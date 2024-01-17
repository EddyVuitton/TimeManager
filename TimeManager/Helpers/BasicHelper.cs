namespace TimeManager.Helpers;

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
            10 => "Paździenik",
            11 => "Listopad",
            12 => "Grudzień",
            _ => string.Empty,
        };
    }
}