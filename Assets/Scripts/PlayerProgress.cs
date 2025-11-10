using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
        string userName = PlayerPrefs.GetString("CurrentUser", "");
        if (string.IsNullOrEmpty(userName))
        {
            Debug.LogWarning("⚠️ No active user found. PlayerProgress will not initialize.");
            Destroy(gameObject);
            return;
        }
        
        string folder = Path.Combine(Application.persistentDataPath, "users", userName);

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        savePath = Path.Combine(folder, "playerProgress.json");
    }

    private void Start()
    {
        LoadProgress();
        UpdateUI();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (levelText == null || xpText == null || xpSlider == null)
        {
            GameObject levelObj = GameObject.Find("LevelText");
            GameObject xpObj = GameObject.Find("XPText");
            GameObject sliderObj = GameObject.Find("XPSlider");

            if (levelObj != null)
                levelText = levelObj.GetComponent<TMP_Text>();

            if (xpObj != null)
                xpText = xpObj.GetComponent<TMP_Text>();

            if (sliderObj != null)
                xpSlider = sliderObj.GetComponent<Slider>();

            UpdateUI();
        }
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

        Debug.Log($"XP added: {amount}, total: {currentXP}/{xpToNextLevel}, Level: {level}");

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
        Debug.Log($"Level Up! You are now Level {level}");
    }

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
        Debug.Log($"Saved Player Progress for {PlayerPrefs.GetString("CurrentUser", "Guest")} to {savePath}");
    }

    public void LoadProgress()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log($"No progress found for {PlayerPrefs.GetString("CurrentUser", "Guest")}, starting fresh.");
            return;
        }

        string json = File.ReadAllText(savePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        currentXP = data.currentXP;
        level = data.level;
        xpToNextLevel = data.xpToNextLevel;

        Debug.Log($"Loaded Progress for {PlayerPrefs.GetString("CurrentUser", "Guest")} — Lv.{level}, XP: {currentXP}/{xpToNextLevel}");
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
