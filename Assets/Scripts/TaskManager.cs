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
        tasks = SaveSystem.LoadTasks();
        if (tasks == null)
            tasks = new List<TaskItem>();

        Debug.Log($"Loaded {tasks.Count} tasks from save file.");

        if (UIManager.Instance != null)
            UIManager.Instance.RefreshTasks();
    }

    private void OnApplicationQuit()
    {
        SaveSystem.SaveTasks(tasks);
        PlayerProgress.Instance.SaveProgress();
    }

    public TaskItem AddTask(string title, string description, DateTime dueDate, int xp = 10)
    {
        TaskItem t = new TaskItem(title, description, dueDate, xp);
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

        if (!t.IsCompleted)
        {
            t.IsCompleted = true;
            SaveSystem.SaveTasks(tasks);
            PlayerProgress.Instance.AddXP(t.xp);
        }

        return true;
    }

    public List<TaskItem> GetAllTasks()
    {
        return tasks;
    }

    public List<TaskItem> GetTasksByDate(DateTime date)
    {
        return tasks.Where(x => x.DueDate.Date == date.Date).ToList();
    }

    public void ClearAllTasks()
    {
        tasks.Clear();
        SaveSystem.ClearSave();

        if (UIManager.Instance != null)
            UIManager.Instance.RefreshTasks();

        Debug.Log(" All tasks cleared!");
    }
}
