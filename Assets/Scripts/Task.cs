using System;


[Serializable]
public class TaskItem
{
    // A runtime unique identifier (string for easy serialization to JSON later)
    public string Id;


    // Basic fields
    public string Title;
    public string Description;
    public DateTime DueDate;
    public bool IsComplete;


    // When the task was created (useful for sorting / audit)
    public DateTime CreatedAt;


    // Constructor used when creating a task from code
    public TaskItem(string title, string description, DateTime dueDate)
    {
        Id = Guid.NewGuid().ToString();
        Title = title;
        Description = description;
        DueDate = dueDate;
        IsComplete = false;
        CreatedAt = DateTime.Now;
    }


    public override string ToString()
    {
        // Friendly debug string used when printing to the console
        return string.Format("[{0}] {1} (Due: {2}) - {3} - Id:{4}",
        IsComplete ? "X" : " ",
        Title,
        DueDate.ToShortDateString(),
        Description,
        Id);
    }
}