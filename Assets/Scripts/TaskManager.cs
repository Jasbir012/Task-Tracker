using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    private List<TaskItem> tasks = new List<TaskItem>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // 🔄 Load saved data at start
        tasks = SaveSystem.LoadTasks();
        if (tasks == null)
            tasks = new List<TaskItem>();

        Debug.Log($"Loaded {tasks.Count} tasks from save file.");

        // Refresh UI after loading
        UIManager.Instance.RefreshTasks();
    }

    private void OnApplicationQuit()
    {
        SaveSystem.SaveTasks(tasks);
    }

    public TaskItem AddTask(string title, string description, DateTime dueDate)
    {
        TaskItem t = new TaskItem(title, description, dueDate);
        tasks.Add(t);
        SaveSystem.SaveTasks(tasks);
        return t;
    }

    public bool MarkTaskComplete(string id)
    {
        var t = tasks.Find(x => x.Id == id);
        if (t == null)
        {
            Debug.LogWarning($"No task found with id {id}");
            return false;
        }

        t.IsComplete = true;
        SaveSystem.SaveTasks(tasks);
        return true;
    }

    // ✅ Get all tasks (no date filtering)
    public List<TaskItem> GetAllTasks()
    {
        return tasks;
    }

    // Optional: Filtered list (for future use)
    public List<TaskItem> GetTasksByDate(DateTime date)
    {
        return tasks.Where(x => x.DueDate.Date == date.Date).ToList();
    }

    public void ClearAllTasks()
    {
        tasks.Clear();
        SaveSystem.ClearSave();
        UIManager.Instance.RefreshTasks();
        Debug.Log("🧹 All tasks cleared!");
    }
}
