using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameManagerScript : Singleton<GameManagerScript>
{
    [SerializeField]
    private IsWinner isWinnerDisplay;
    public static bool GameGoingOn = false;

    public void gameEnd(Player winner)
    {
        isWinnerDisplay.GameEnd(winner);
    }
}
