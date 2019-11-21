using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PauseController : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject PauseMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
                if (isPaused == false)
                {
                    Time.timeScale = 0.0f;
                    PauseMenu.gameObject.SetActive(true);
                    isPaused = true;
                }
                else
                {
                    Time.timeScale = 1.0f;
                    PauseMenu.gameObject.SetActive(false);
                    isPaused = false;
                }
            }
    }
}
