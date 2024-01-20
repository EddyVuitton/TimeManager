namespace TimeManager.Components.Pages;

public partial class Index
{
    private readonly DateTime _now = DateTime.Now;
    private readonly List<DateTime> _firstDayInMonths = new();
    private readonly List<int> _years = new();

    private int _minYear;
    private int _maxYear;
    private DateTime _selectedMonthAndYear;

    protected override void OnInitialized()
    {
        InitFields();
    }

    #region PrivateMethods
    private void InitFields()
    {
        _minYear = 1000;
        _maxYear = 9999;
        _selectedMonthAndYear = new DateTime(_now.Year, _now.Month, 1);

        FillUpLists();
    }

    private void FillUpLists()
    {
        for (int year = _minYear; year <= _maxYear; year++) //Add years
        {
            _years.Add(year);

            for (int month = 1; month <= 12; month++) //Add first day in months
            {
                _firstDayInMonths.Add(new DateTime(year, month, 1));
            }
        }
    }
    #endregion PrivateMethods
}