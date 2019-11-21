using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsWinner : MonoBehaviour
{
    public GameObject playerWin1;
    public GameObject playerWin2;
    public GameObject endMenu;

    public void GameEnd(Player winner)
    {
        GameManagerScript.GameGoingOn = false;
        StartCoroutine(DisplayGameEnd(winner));
    }

    IEnumerator DisplayGameEnd(Player winner)
    {
        GameManagerScript.GameGoingOn = false;
        yield return new WaitForSecondsRealtime(1);
        yield return new WaitForSecondsRealtime(0.5f);
        if (winner == Player.player1)
        { 
            playerWin1.gameObject.SetActive(true);
        }
        else
        {
            playerWin2.gameObject.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        endMenu.gameObject.SetActive(true);
    }
}
