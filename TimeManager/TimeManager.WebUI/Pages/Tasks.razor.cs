namespace TimeManager.WebUI.Pages;

public partial class Tasks
{
    private readonly List<CustomList> lists = [];

    protected override void OnInitialized()
    {
        for (int i = 0; i < 1; i++)
        {
            lists.Add(new CustomList() { ID = i, IsChecked = false });
        }
    }

    private void AddList()
    {
        var newId = lists.Max(x => x.ID) + 1;

        lists.Add(new CustomList() { ID = newId, IsChecked = false });
    }

    private void AddTask(int id)
    {
        lists.First(x => x.ID == id).Tasks.Add("Zadanie");
    }

    private class CustomList
    {
        public int ID { get; init; }
        public bool IsChecked { get; set; }

        public List<string> Tasks { get; set; } = [];
    }
}