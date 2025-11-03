using System;

[Serializable]
public class TaskItem
{
    public string Id;
    public string Title;
    public string Description;
    public bool IsCompleted;
    public int xp;
    public string DueDateString;
    public DateTime CreatedAt;

    public DateTime DueDate
    {
        get
        {
            if (DateTime.TryParse(DueDateString, out DateTime parsed))
                return parsed;
            return DateTime.Now;
        }
        set
        {
            DueDateString = value.ToString("yyyy-MM-dd");
        }
    }

    public TaskItem(string title, string description, DateTime dueDate, int xp)
    {
        Id = Guid.NewGuid().ToString();
        Title = title;
        Description = description;
        DueDate = dueDate;
        IsCompleted = false;
        this.xp = xp;
        CreatedAt = DateTime.Now;
    }
}
