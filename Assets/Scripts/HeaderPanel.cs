using UnityEngine;
using TMPro;
using System;
using System.IO;

public class HeaderPanel : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text greetingText;
    public TMP_Text dateText;

    private string userName = "Player";

    void Start()
    {
        LoadUserName();
        UpdateDate();
        UpdateGreeting();
    }

    void LoadUserName()
    {
        // ✅ Get current active user
        if (PlayerPrefs.HasKey("CurrentUser"))
        {
            userName = PlayerPrefs.GetString("CurrentUser");
        }
        else
        {
            // Backup option: read from user.json if PlayerPrefs was cleared
            string basePath = Path.Combine(Application.persistentDataPath, "users");
            if (Directory.Exists(basePath))
            {
                string[] folders = Directory.GetDirectories(basePath);
                if (folders.Length > 0)
                {
                    string lastUserPath = Path.Combine(folders[folders.Length - 1], "user.json");
                    if (File.Exists(lastUserPath))
                    {
                        string json = File.ReadAllText(lastUserPath);
                        UserData data = JsonUtility.FromJson<UserData>(json);
                        userName = data.userName;
                    }
                }
            }
        }
    }

    void UpdateDate()
    {
        if (dateText != null)
            dateText.text = DateTime.Now.ToString("dddd, dd MMM yyyy");
    }

    void UpdateGreeting()
    {
        int hour = DateTime.Now.Hour;
        string greeting;

        if (hour < 12)
            greeting = "Good Morning";
        else if (hour < 18)
            greeting = "Good Afternoon";
        else
            greeting = "Good Evening";

        if (greetingText != null)
            greetingText.text = $"{greeting}, {userName}!";
    }

    [System.Serializable]
    private class UserData
    {
        public string userName;
    }
}
