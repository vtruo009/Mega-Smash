using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public void startButton()
    {
            SceneManager.LoadScene("Fighting");
    }
    public void goBackToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void pauseBackToMenu()
    {
        Time.timeScale = 1.0f;
        PauseController.isPaused = false;
        SceneManager.LoadScene("Main Menu");
    }
}
