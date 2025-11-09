using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class LogoutButton : MonoBehaviour
{
    public void Logout()
    {
        
        PlayerPrefs.DeleteKey("CurrentUser");
        PlayerPrefs.Save(); 

        Debug.Log("Logged out. Session cleared.");

        SceneManager.LoadScene("LoginScene");
    }
}
