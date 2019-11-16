using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player { player1, player2};
public enum State { alive, punch, fire, dead};
[System.Serializable]
public class CharacterControll : MonoBehaviour
{
    private Transform transform;
    private const float speed = 0.3f;

    private Rigidbody2D rigidBody;
    private const float pushForce = 500f;

    private const float attackDuration = 0.5f; // seconds

    private const float maxHealth = 100;
    private float health;

    Controller controller;

    private const float jumpTime = 0.5f;
    private bool canJump;

    private const float seaLevel = -3f;

    private State state;

    [SerializeField]
    public Player player;

    private Quaternion faceRight;
    private Quaternion faceLeft;

    private void Start()
    {
        transform = GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody2D>();

        if (player == Player.player1)
        {
            controller = new Controller(
                KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.LeftArrow,
                KeyCode.K, KeyCode.DownArrow, KeyCode.L);
        }
        else
        {
            controller = new Controller(
            KeyCode.W, KeyCode.D, KeyCode.A,
            KeyCode.F, KeyCode.S, KeyCode.G);
        }

        faceRight = new Quaternion(0f, -180f, 0f, 0f);
        faceLeft = new Quaternion(0f, 0f, 0f, 0f);

        canJump = true;
        state = State.alive;
        health = maxHealth;
    }
    
    private void Update()
    {
        if (transform.position.y < seaLevel && state != State.dead)
        {
            Debug.Log("You lost :)");
            state = State.dead;
        }

        if (Input.GetKey(controller.right))
        {
            transform.rotation = faceRight;
            transform.position += new Vector3(speed, 0f, 0f);
        }

        if (Input.GetKey(controller.left))
        {
            transform.rotation = faceLeft;
            transform.position += new Vector3(speed * -1f, 0f, 0f);
        }

        if (Input.GetKeyDown(controller.jump) && canJump){
            StartCoroutine(Jump());
        }

        if (Input.GetKeyDown(controller.crouch))
        {
            Vector3 scale = transform.localScale;
            scale.y /= 2;
            transform.localScale = scale;
        }

        if (Input.GetKeyUp(controller.crouch))
        {
            Vector3 scale = transform.localScale;
            scale.y *= 2;
            transform.localScale = scale;
        }

        if (Input.GetKeyDown(controller.punch))
        {
            StartCoroutine(Punch());
        }

        if (Input.GetKeyDown(controller.fire))
        {
            StartCoroutine(Fire());
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "character")
        {
            CharacterControll opponent = collision.gameObject.GetComponent<CharacterControll>();

            if (opponent.state == State.fire)
            {
                health -= 10;
            }
            else if (opponent.state == State.punch)
            {
                health -= 5;
            }

            if (health <= 0)
            {
                state = State.dead;
            }
        }
    }

    #region IEnumerators
    IEnumerator Punch()
    {
        state = State.fire;
        Vector3 scale = transform.localScale;
        scale.x *= 2;
        transform.localScale = scale;

        yield return new WaitForSeconds(attackDuration);

        scale = transform.localScale;
        scale.x /= 2;
        transform.localScale = scale;
        state = State.alive;
    }

    IEnumerator Jump()
    {
        rigidBody.AddForce(new Vector3(0f, pushForce, 0f));
        canJump = false;

        yield return new WaitForSeconds(jumpTime);

        canJump = true;
    }

    IEnumerator Fire()
    {
        state = State.fire;

        yield return new WaitForSeconds(jumpTime);

        canJump = true;
        state = State.alive;
    }
    #endregion
}

public class Controller
{
    public KeyCode jump;
    public KeyCode right;
    public KeyCode left;
    public KeyCode punch;
    public KeyCode crouch;
    public KeyCode fire;

    public Controller(KeyCode jump, KeyCode right, KeyCode left, KeyCode punch, KeyCode crouch, KeyCode fire)
    {
        this.jump = jump;
        this.right = right;
        this.left = left;
        this.punch = punch;
        this.crouch = crouch;
        this.fire = fire;
    }
}
