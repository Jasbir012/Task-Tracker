using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapManager : MonoBehaviour
{
    private void Start()
    {
        
        Invoke(nameof(CheckLoginStatus), 0.05f);
    }

    void CheckLoginStatus()
    {
       
        if (PlayerPrefs.HasKey("CurrentUser"))
        {
            string currentUser = PlayerPrefs.GetString("CurrentUser", "");
            if (!string.IsNullOrEmpty(currentUser))
            {
                Debug.Log($" User '{currentUser}' found — loading MainScene...");
                SceneManager.LoadScene("SampleScene"); 
                return;
            }
        }

        Debug.Log(" No active user found,  redirecting to LoginScene...");
        SceneManager.LoadScene("LoginScene");
    }
}
