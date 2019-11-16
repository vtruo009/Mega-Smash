using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player { player1, player2};
public enum State { alive, dead};
[System.Serializable]
public class CharacterControll : MonoBehaviour
{
    private Transform transform;
    private const float speed = 0.3f;

    private Rigidbody2D rigidBody;
    private const float pushForce = 500f;

    private const float attackDuration = 0.5f; // seconds

    public float health;
    
    private KeyCode jump;

    CharacterControl characterControl;

    private const float jumpTime = 0.5f;
    private bool canJump;

    private const float seaLevel = -2.5f;

    private State state;

    [SerializeField]
    public Player player;

    private void Start()
    {
        transform = GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody2D>();

        if (player == Player.player1)
        {
            characterControl = new CharacterControl(
                KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.LeftArrow,
                KeyCode.K, KeyCode.DownArrow, KeyCode.L);
        }
        else
        {
            characterControl = new CharacterControl(
            KeyCode.W, KeyCode.D, KeyCode.A,
            KeyCode.F, KeyCode.S, KeyCode.G);
        }
        canJump = true;
        state = State.alive;
    }
    
    private void Update()
    {
        if (transform.position.y < seaLevel && state != State.dead)
        {
            Debug.Log("You lost :)");
            state = State.dead;
        }

        if (Input.GetKey(characterControl.right))
        {
            transform.position += new Vector3(speed, 0f, 0f);
        }

        if (Input.GetKey(characterControl.left))
        {
            transform.position += new Vector3(speed * -1f, 0f, 0f);
        }

        if (Input.GetKeyDown(characterControl.jump) && canJump){
            rigidBody.AddForce(new Vector3(0f, pushForce, 0f));
            StartCoroutine(Jump());
        }

        if (Input.GetKeyDown(characterControl.crouch))
        {
            Vector3 scale = transform.localScale;
            scale.y /= 2;
            transform.localScale = scale;
        }
        if (Input.GetKeyUp(characterControl.crouch))
        {
            Vector3 scale = transform.localScale;
            scale.y *= 2;
            transform.localScale = scale;
        }

        if (Input.GetKeyDown(characterControl.punch))
        {
            StartCoroutine(Attack());
        }

    }

    IEnumerator Attack()
    {
        Vector3 scale = transform.localScale;
        scale.x *= 2;
        transform.localScale = scale;

        yield return new WaitForSeconds(attackDuration);

        scale = transform.localScale;
        scale.x /= 2;
        transform.localScale = scale;
    }

    IEnumerator Jump()
    {
        canJump = false;

        yield return new WaitForSeconds(jumpTime);

        canJump = true;
    }
}

public class CharacterControl
{
    public KeyCode jump;
    public KeyCode right;
    public KeyCode left;
    public KeyCode punch;
    public KeyCode crouch;
    public KeyCode fire;

    public CharacterControl(KeyCode jump, KeyCode right, KeyCode left, KeyCode punch, KeyCode crouch, KeyCode fire)
    {
        this.jump = jump;
        this.right = right;
        this.left = left;
        this.punch = punch;
        this.crouch = crouch;
        this.fire = fire;
    }
}
