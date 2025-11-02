using System;

[Serializable]
public class TaskItem
{
    public string Id;
    public string Title;
    public string Description;
    public bool IsComplete;

    // Store date as ISO string for JSON
    public string DueDateString;

    // Helper property (not serialized directly)
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

    public DateTime CreatedAt;

    public TaskItem(string title, string description, DateTime dueDate)
    {
        Id = Guid.NewGuid().ToString();
        Title = title;
        Description = description;
        DueDate = dueDate;
        IsComplete = false;
        CreatedAt = DateTime.Now;
    }
}
