using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string filePath = Path.Combine(Application.persistentDataPath, "tasks.json");

    public static void SaveTasks(List<TaskItem> tasks)
    {
        string json = JsonUtility.ToJson(new TaskListWrapper { tasks = tasks }, true);
        File.WriteAllText(filePath, json);
        Debug.Log($"💾 Saved {tasks.Count} tasks to {filePath}");
    }

    public static List<TaskItem> LoadTasks()
    {
        if (!File.Exists(filePath))
        {
            Debug.Log("No save file found.");
            return new List<TaskItem>();
        }

        string json = File.ReadAllText(filePath);
        TaskListWrapper wrapper = JsonUtility.FromJson<TaskListWrapper>(json);
        return wrapper?.tasks ?? new List<TaskItem>();
    }

    public static void ClearSave()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("🗑 Save file deleted!");
        }
    }

    [System.Serializable]
    private class TaskListWrapper
    {
        public List<TaskItem> tasks;
    }
}
