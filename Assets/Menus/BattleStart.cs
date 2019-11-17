using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStart : MonoBehaviour
{
    public GameObject readyText;
    public GameObject wallopText;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0.0f;
        Coroutine StartWait = StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(1);
        readyText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        readyText.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.5f);
        wallopText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        wallopText.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1.0f;
    }
}
