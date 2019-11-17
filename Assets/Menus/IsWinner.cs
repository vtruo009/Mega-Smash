using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsWinner : MonoBehaviour
{
    bool gameOver = false;
    public int health1;
    public int health2;
    public GameObject knockoutText;
    public GameObject playerWin1;
    public GameObject playerWin2;
    public GameObject endMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver == false)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                health1--;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                health2--;
            }
            if (health1 < 1)
            {
                knockoutText.gameObject.SetActive(true);
                Coroutine StartWait = StartCoroutine(Wait());
            }
            else if (health2 < 1)
            {
                knockoutText.gameObject.SetActive(true);
                Coroutine StartWait = StartCoroutine(Wait());
            }
        }
        else
        {

        }
    }

    IEnumerator Wait()
    {
        gameOver = true;
        yield return new WaitForSecondsRealtime(1);
        knockoutText.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.5f);
        if (health1 < 1)
        {
            playerWin2.gameObject.SetActive(true);
        }
        else if (health2 < 1)
        {
            playerWin1.gameObject.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        endMenu.gameObject.SetActive(true);
    }
}
