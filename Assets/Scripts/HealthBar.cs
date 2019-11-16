using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider player1Health;
    public Slider player2Health;
    public CharacterControll hp1;
    public CharacterControll hp2;

    // Start is called before the first frame update
    void Start()
    {
        player1Health.value = player2Health.value = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (player1Health.value != hp1.health) {
            player1Health.value = hp1.health;
        }
        if (player2Health.value != hp2.health) {
            player2Health.value = hp2.health;
        }
    }
}
