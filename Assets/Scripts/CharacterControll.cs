using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player { player1, player2};
public enum State { alive, punch, fire, hurt, dead};
public enum Direction { right, left};
[System.Serializable]
public class CharacterControll : MonoBehaviour
{
    private Transform transform;
    private const float speed = 0.3f;

    private SpriteRenderer spriteRenderer;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip fire;
    [SerializeField]
    private AudioClip punch;
    [SerializeField]
    private AudioClip jump;


    private Animator animator;
    private const string animatorPunchTrigger = "Punch";
    private const string animatorDamageTrigger = "Damage";
    private const string animatorDeadTrigger = "Dead";
    private const string animatorFireTrigger = "Fire";

    private Rigidbody2D rigidBody;
    private const float jumpForce = 500f;
    private const float punchForce = 300f;

    private const float attackDuration = 0.5f; // seconds
    private const float hurtDuration = 0.3f;

    public float health;
    private const float maxHealth = 100;

    Controller controller;

    private const float jumpTime = 0.5f;
    private bool canJump;

    private const float seaLevel = -5f;

    private State state;

    [SerializeField]
    public Player player;
    [SerializeField]
    public Sprite defaultSprite;
    [SerializeField]
    public Sprite hurtSprite;

    [SerializeField]
    public GameObject fireBall;

    private Direction direction;
 
    private Quaternion faceRight;
    private Quaternion faceLeft;

    private void Start()
    {
        transform = GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (player == Player.player1)
        {
            controller = new Controller(
                KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.LeftArrow,
                KeyCode.K, KeyCode.DownArrow, KeyCode.L);
            direction = Direction.left;
        }
        else
        {
            controller = new Controller(
                KeyCode.W, KeyCode.D, KeyCode.A,
                KeyCode.F, KeyCode.S, KeyCode.G);
            direction = Direction.right;
        }

        faceRight = new Quaternion(0f, -180f, 0f, 0f);
        faceLeft = new Quaternion(0f, 0f, 0f, 0f);

        canJump = true;
        state = State.alive;
        health = maxHealth;
    }
    
    private void Update()
    {
        if (state == State.hurt) return;
        if (state == State.dead) return;

        if (transform.position.y < seaLevel && state != State.dead)
        {
            Debug.Log("You lost :)");
            state = State.dead;
        }

        if (Input.GetKey(controller.right))
        {
            transform.rotation = faceRight;
            transform.position += new Vector3(speed, 0f, 0f);
            direction = Direction.right;
        }

        if (Input.GetKey(controller.left))
        {
            transform.rotation = faceLeft;
            transform.position += new Vector3(speed * -1f, 0f, 0f);
            direction = Direction.left;
        }

        if (Input.GetKeyDown(controller.jump) && canJump){
            audioSource.PlayOneShot(jump);
            StartCoroutine(Jump());
        }

        if (Input.GetKeyDown(controller.crouch))
        {
            Vector3 scale = transform.localScale;
            scale.y = 1;
            transform.localScale = scale;
        }

        if (Input.GetKeyUp(controller.crouch))
        {
            Vector3 scale = transform.localScale;
            scale.y = 2;
            transform.localScale = scale;
        }

        if (Input.GetKeyDown(controller.punch) && state != State.punch && state != State.hurt)
        {
            audioSource.PlayOneShot(punch);
            StartCoroutine(Punch());
        }

        if (Input.GetKeyDown(controller.fire) && state != State.hurt && state != State.fire)
        {
            audioSource.PlayOneShot(fire);
            StartCoroutine(Fire());
        }

    }

    private void DecreaseHealth(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            state = State.dead;
            animator.SetTrigger(animatorDeadTrigger);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.gameObject.tag == "fire")
        {
            GameObject fireGameObject = collider.gameObject;
            if (fireGameObject.GetComponent<FireBall>().player != this.player)
            {
                CharacterControll opponent = fireGameObject.GetComponent<CharacterControll>();
                Transform opponentTransform = fireGameObject.GetComponent<Transform>();

                rigidBody.AddForce(
                               new Vector3(
                                   punchForce * ((transform.position.x > opponentTransform.position.x) ? 1f : -1f),
                                   0f,
                                   0f)
                               );

                DecreaseHealth(10);
                state = State.hurt;
                StartCoroutine(Hurt());
                Destroy(fireGameObject.gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "character")
        {
            GameObject opponentGameObject = collision.gameObject;
            CharacterControll opponent = opponentGameObject.GetComponent<CharacterControll>();
            Transform opponentTransform = opponentGameObject.GetComponent<Transform>();
           
            if (opponent.state == State.punch)
            {
                rigidBody.AddForce(
                    new Vector3(
                        punchForce * ((transform.position.x > opponentTransform.position.x) ? 1f : -1f),
                        0f,
                        0f)
                    );
                state = State.hurt;
                StartCoroutine(Hurt());
                DecreaseHealth(20);
            }
        }

        
    }

    #region IEnumerators
    IEnumerator Hurt()
    {
        animator.SetBool(animatorDamageTrigger, true);
        yield return new WaitForSeconds(hurtDuration);
        if (state == State.dead) yield return null;

        state = State.alive;
        animator.SetBool(animatorDamageTrigger, false);
    }

    IEnumerator Punch()
    {
        state = State.punch;
        animator.SetTrigger(animatorPunchTrigger);

        yield return new WaitForSeconds(attackDuration);
        
        state = State.alive;
    }

    IEnumerator Jump()
    {
        rigidBody.AddForce(new Vector3(0f, jumpForce, 0f));
        canJump = false;

        yield return new WaitForSeconds(jumpTime);

        canJump = true;
    }

    IEnumerator Fire()
    {
        state = State.fire;
        animator.SetTrigger(animatorFireTrigger);
        GameObject ballOfFire = Instantiate(fireBall, transform.position, Quaternion.identity);
        ballOfFire.GetComponent<FireBall>().setPosition(direction == Direction.right, player);


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
