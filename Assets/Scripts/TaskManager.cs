using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance; // Singleton access

    private List<TaskItem> tasks = new List<TaskItem>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debug.Log("TaskManager initialized");
    }

    // Add a new task
    public TaskItem AddTask(string title, string description, DateTime dueDate)
    {
        var t = new TaskItem(title, description, dueDate);
        tasks.Add(t);
        Debug.Log($"Added task: {t.Title} (id: {t.Id})");
        return t;
    }

    // Mark as complete
    public bool MarkTaskComplete(string id)
    {
        var t = tasks.Find(x => x.Id == id);
        if (t == null) return false;

        t.IsComplete = true;
        return true;
    }

    // Get all tasks for a specific date
    public List<TaskItem> GetTasksByDate(DateTime date)
    {
        return tasks.Where(x => x.DueDate.Date == date.Date).ToList();
    }

    // Get all tasks
    public List<TaskItem> GetAllTasks()
    {
        return tasks;
    }
}
