using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField nameInput;
    private string baseUserPath;

    [System.Serializable]
    private class UserData
    {
        public string userName;
    }

    private void Start()
    {
        baseUserPath = Path.Combine(Application.persistentDataPath, "users");

        if (!Directory.Exists(baseUserPath))
            Directory.CreateDirectory(baseUserPath);
    }

    public void OnContinueClicked()
    {
        string name = nameInput.text.Trim();
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning(" Please enter your name!");
            return;
        }

        string userFolder = Path.Combine(baseUserPath, name);
        if (!Directory.Exists(userFolder))
            Directory.CreateDirectory(userFolder);

        
        UserData data = new UserData { userName = name };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Path.Combine(userFolder, "user.json"), json);

        
        PlayerPrefs.SetString("CurrentUser", name);

        SceneManager.LoadScene("SampleScene");
    }
}
