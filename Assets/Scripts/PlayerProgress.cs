using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance;

    [Header("Player Stats")]
    public int currentXP = 0;
    public int level = 1;
    public int xpToNextLevel = 100;

    [Header("UI Elements")]
    public TMP_Text levelText;
    public TMP_Text xpText;
    public Slider xpSlider;

    private string savePath;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        savePath = Path.Combine(Application.persistentDataPath, "playerProgress.json");
    }

    private void Start()
    {
        LoadProgress();
        UpdateUI();
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        if (currentXP < 0) currentXP = 0;

        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            level++;
            xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f);
            LevelUpFeedback();
        }

        UpdateUI();
        SaveProgress();
    }

    private void UpdateUI()
    {
        if (levelText != null)
            levelText.text = "Lv. " + level;

        if (xpText != null)
            xpText.text = $"{currentXP} / {xpToNextLevel} XP";

        if (xpSlider != null)
        {
            xpSlider.maxValue = xpToNextLevel;
            xpSlider.value = currentXP;
        }
    }

    private void LevelUpFeedback()
    {
        Debug.Log($"🎉 Level Up! You are now Level {level}");
        // Optional: add particle/sound effects here
    }

    // ---------- SAVE / LOAD ----------
    [System.Serializable]
    private class PlayerData
    {
        public int currentXP;
        public int level;
        public int xpToNextLevel;
    }

    public void SaveProgress()
    {
        PlayerData data = new PlayerData
        {
            currentXP = currentXP,
            level = level,
            xpToNextLevel = xpToNextLevel
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"💾 Saved Player Progress to {savePath}");
    }

    public void LoadProgress()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No player progress file found, starting fresh.");
            return;
        }

        string json = File.ReadAllText(savePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        currentXP = data.currentXP;
        level = data.level;
        xpToNextLevel = data.xpToNextLevel;

        Debug.Log($"📂 Loaded Player Progress — Lv.{level}, XP: {currentXP}/{xpToNextLevel}");
    }

    public void ResetProgress()
    {
        currentXP = 0;
        level = 1;
        xpToNextLevel = 100;
        SaveProgress();
        UpdateUI();
    }
}
