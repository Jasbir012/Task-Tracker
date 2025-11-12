using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapManager : MonoBehaviour
{
    private void Start()
    {
        Debug.Log(" Bootstrap started. Checking login status...");
        PlayerPrefs.Save();

        Invoke(nameof(CheckLoginStatus), 0.1f);
    }

    void CheckLoginStatus()
    {
        
        if (PlayerPrefs.HasKey("CurrentUser"))
        {
            string currentUser = PlayerPrefs.GetString("CurrentUser", "");
            Debug.Log($" CurrentUser found in prefs: '{currentUser}'");

           
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

       
        Debug.Log(" No valid user session found. Redirecting to LoginScene...");
        SceneManager.LoadScene("LoginScene");
    }
}
