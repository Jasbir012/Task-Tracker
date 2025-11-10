using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string GetUserFilePath()
    {
        if (!PlayerPrefs.HasKey("CurrentUser"))
        {
            Debug.LogWarning(" No logged-in user found. SaveSystem will not load or save data.");
            return null;
        }

        string userName = PlayerPrefs.GetString("CurrentUser", "");
        if (string.IsNullOrEmpty(userName))
        {
            Debug.LogWarning(" Invalid user name in PlayerPrefs. Skipping save/load.");
            return null;
        }

        string folder = Path.Combine(Application.persistentDataPath, "users", userName);
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        return Path.Combine(folder, "tasks.json");
    }

    public static void SaveTasks(List<TaskItem> tasks)
    {
        string path = GetUserFilePath();
        if (string.IsNullOrEmpty(path)) return;

        string json = JsonUtility.ToJson(new TaskListWrapper { tasks = tasks }, true);
        File.WriteAllText(path, json);
        Debug.Log($"Saved {tasks.Count} tasks for {PlayerPrefs.GetString("CurrentUser")}");
    }

    public static List<TaskItem> LoadTasks()
    {
        string path = GetUserFilePath();
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            Debug.Log(" No valid user or task file found. Returning empty list.");
            return new List<TaskItem>();
        }

        string json = File.ReadAllText(path);
        TaskListWrapper wrapper = JsonUtility.FromJson<TaskListWrapper>(json);
        return wrapper?.tasks ?? new List<TaskItem>();
    }

    public static void ClearSave()
    {
        string path = GetUserFilePath();
        if (string.IsNullOrEmpty(path)) return;

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log(" Cleared user tasks!");
        }
    }

    [System.Serializable]
    private class TaskListWrapper
    {
        public List<TaskItem> tasks;
    }
}
