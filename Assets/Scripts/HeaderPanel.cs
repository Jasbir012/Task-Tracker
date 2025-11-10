using UnityEngine;
using TMPro;
using System;

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
        // ✅ Only load if a real logged-in user exists
        if (PlayerPrefs.HasKey("CurrentUser"))
        {
            userName = PlayerPrefs.GetString("CurrentUser");
        }
        else
        {
            Debug.LogWarning(" No CurrentUser found — skipping user load.");
            userName = "Guest";
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
}
