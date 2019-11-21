using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameChanging : MonoBehaviour
{
    public RawImage names;
    public int nameValue;
    int nameCurrent;

    void Start()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
        nameCurrent = 3;
        InvokeRepeating("Changename", 0.1f, 1.0f);
    }

    void Changename()
    {
        if (nameCurrent == 0)
        {
            nameCurrent++;
        }
        else if (nameCurrent == 1)
        {
            nameCurrent++;
        }
        else if (nameCurrent == 2)
        {
            nameCurrent++;
        }
        else if (nameCurrent == 3)
        {
            nameCurrent = 0;
        }
        if (nameCurrent == nameValue)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(0, 0, 0);
        }
    }
}