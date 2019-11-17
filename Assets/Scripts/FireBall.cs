using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private float speed = 0.4f;
    Transform transform;

    public Player player;

    private void Start()
    {
        transform = GetComponent<Transform>();
        StartCoroutine(DestroyThis());
    }
    
    private void Update()
    {
        Vector3 position = transform.position;
        position.x += speed;
        transform.position = position;
    }

    public void setPosition(bool isRight, Player player)
    {
        if (!isRight)
        {
            speed = speed * -1f;
        }

        this.player = player;
    }

    IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
