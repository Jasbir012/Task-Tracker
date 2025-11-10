using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapManager : MonoBehaviour
{
    private void Start()
    {
        Debug.Log(" Bootstrap started. Checking login status...");
        PlayerPrefs.Save(); // Make sure it's synced

        Invoke(nameof(CheckLoginStatus), 0.1f);
    }

    void CheckLoginStatus()
    {
        // Force check what’s actually in PlayerPrefs
        if (PlayerPrefs.HasKey("CurrentUser"))
        {
            string currentUser = PlayerPrefs.GetString("CurrentUser", "");
            Debug.Log($" CurrentUser found in prefs: '{currentUser}'");

            // If we find an empty or Guest value, treat as no user
            if (!string.IsNullOrEmpty(currentUser) && currentUser != "Guest")
            {
                Debug.Log($" User '{currentUser}' found — loading SampleScene...");
                SceneManager.LoadScene("SampleScene");
                return;
            }
        }
        else
        {
            Debug.Log("layerPrefs has no 'CurrentUser' key.");
        }

        // Fallback always goes to Login
        Debug.Log(" No valid user session found. Redirecting to LoginScene...");
        SceneManager.LoadScene("LoginScene");
    }
}
