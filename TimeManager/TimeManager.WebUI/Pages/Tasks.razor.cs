using Microsoft.AspNetCore.Components;
using MudBlazor;
using TimeManager.WebUI.Dialogs;

namespace TimeManager.WebUI.Pages;

public partial class Tasks
{
    [Inject] public IDialogService DialogService { get; private init; } = null!;

    private readonly List<CustomList> lists = [];

    protected override void OnInitialized()
    {
        lists.Add(new CustomList() { ID = 0, Name = "Moje zadania", IsChecked = false });
    }

    #region PrivateMethods

    private void AddTask(int id)
    {
        lists.First(x => x.ID == id).Tasks.Add("Zadanie");
    }

    private void OpenAddListDialog()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true
        };

        var parameters = new DialogParameters
            {
                { "TasksRef", this }
            };

        DialogService.Show<AddListDialog>("Tworzenie nowej listy", parameters, options);
    }

    private class CustomList
    {
        public int ID { get; init; }
        public string Name { get; set; } = string.Empty;
        public bool IsChecked { get; set; }

        public List<string> Tasks { get; set; } = [];
    }

    #endregion PrivateMethods

    #region PublicMethods

    public void AddList(string name)
    {
        var newId = lists.Max(x => x.ID) + 1;
        lists.Add(new CustomList() { ID = newId, Name = name, IsChecked = false });

        StateHasChanged();
    }

    #endregion PublicMethods
}