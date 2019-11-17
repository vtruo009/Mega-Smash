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
}
