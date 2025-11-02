using UnityEngine;
using TMPro;
using System;

public class HeaderPanel : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text greetingText;
    public TMP_Text dateText;

    [Header("User Info")]
    public string userName = "NightMare";

    void Start()
    {
        UpdateDate();
        UpdateGreeting();
    }

    void UpdateDate()
    {
        dateText.text = DateTime.Now.ToString("dddd, dd MMM yyyy");
    }

    void UpdateGreeting()
    {
        int hour = DateTime.Now.Hour;
        string greeting;

        if (hour < 12)
            greeting = "Good Morning 🌞";
        else if (hour < 18)
            greeting = "Good Afternoon ☀️";
        else
            greeting = "Good Evening 🌙";

        greetingText.text = $"{greeting}, {userName}!";
    }
}
