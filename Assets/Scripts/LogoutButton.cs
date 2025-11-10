using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutButton : MonoBehaviour
{
    public void Logout()
    {
        Debug.Log(" Logging out — clearing session data...");

        PlayerPrefs.DeleteKey("CurrentUser");
        PlayerPrefs.Save();

        if (PlayerProgress.Instance != null)
        {
            PlayerProgress.Instance.ResetProgress();
            Destroy(PlayerProgress.Instance.gameObject);
        }

        System.Threading.Thread.Sleep(100);
        SceneManager.LoadScene("LoginScene");
    }
}
