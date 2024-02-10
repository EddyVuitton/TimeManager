namespace TimeManager.Domain.Context;

public static class SeedDataService
{
    public static void Initialize(DBContext context)
    {
        //zapisz zmiany w bazie danych
        context.SaveChanges();
    }
}