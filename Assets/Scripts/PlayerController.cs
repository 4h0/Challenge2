using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Sprite[] walkingLeft;
    public Sprite[] walkingRight;
    public Sprite[] jumpingLeft;
    public Sprite[] jumpingRight;

    public int numberOfCoin;
    public int health;

    public float speed;
    public float jumpHeight;
    public bool dying;

    private SpriteRenderer showPlayer;
    private Collider2D playerCollider2D;
    private Rigidbody2D playerRigidBody2D;

    private Vector2 originalPosition;
    private Vector2 movement;

    private AudioSource soundSource;
    private AudioClip[] soundEffect;

    private Sprite currentSprite;
    private Sprite[] walkingSprite;
    private Sprite[] jumpingSprite;

    private bool canMove;
    private bool onGround;
    private bool nearEnemy;
    private bool soundPlayed;
    private bool canChangeSprite;
    private bool faceRight;

    private int animationCounter;
    private int lowWalkingCounter;
    private int maxWalkingCounter;
    private int lowJumpingCounter;
    private int maxJumpingCounter;

    private float healthDecreaseCountDown;
    private float moveHorizontal;

    private void Awake()
    {
        showPlayer = GetComponent<SpriteRenderer>();
        playerCollider2D = GetComponent<Collider2D>();
        playerRigidBody2D = GetComponent<Rigidbody2D>();
        soundSource = GetComponent<AudioSource>();
        
        originalPosition = this.transform.position;

        canMove = true;
        dying = false;
        nearEnemy = false;
        soundPlayed = false;
        canChangeSprite = true;
        faceRight = true;

        animationCounter = 0;
        health = 1;healthDecreaseCountDown = 0f;
    }

    private void Start()
    {
        soundEffect = new AudioClip[]
        {
            (AudioClip)Resources.Load("Sound/Jump11"),            
            (AudioClip)Resources.Load("Sound/Pickup_Coin6"),
            (AudioClip)Resources.Load("Sound/Powerup4")
        };

        walkingSprite = new Sprite[walkingRight.Length];
        jumpingSprite = new Sprite[jumpingRight.Length];
        SetAnimationSprite();
    }

    private void Update()
    {
        //movement logic
        if (canMove)
        {
            if (playerRigidBody2D.velocity.y < 0)
            {
                playerRigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (jumpHeight - 1) * Time.deltaTime;
            }

            if (!onGround)
            {
                if (playerRigidBody2D.gravityScale <= 3)
                {
                    playerRigidBody2D.gravityScale += .6f;
                }
                /*
                if(playerRigidBody2D.gravityScale > 3)
                {
                    StartCoroutine(ReturnToNormalGravity());
                }
                */

                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    playerRigidBody2D.gravityScale = 24f;
                }
            }

            if (onGround)
            {
                playerRigidBody2D.gravityScale = 1f;

                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
                {
                    soundSource.clip = soundEffect[0];

                    if (!soundPlayed)
                    {
                        soundPlayed = true;
                    }

                    playerRigidBody2D.gravityScale = -7f;
                    //playerRigidBody2D.velocity = transform.up * jumpHeight * Time.deltaTime;
                    //playerRigidBody2D.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);

                    StartCoroutine(JumpingAnimation());
                }
            }

            if(soundPlayed)
            {
                StartCoroutine(PlaySound());
            }
        }
        else
        {
            if (!dying)
            {
                StartCoroutine(PlayerDie());
            }
        }

        //stuck near enemy logic
        if(nearEnemy)
        {
            healthDecreaseCountDown += .1f;
        }
        else
        {
            healthDecreaseCountDown = 0;
        }

        if(healthDecreaseCountDown > 6)
        {
            health--;
        }

        //animation part
        ChangeSpriteBasedOnHealth();
        CheckFacingDirection();
        SetAnimationSprite();
        
        if (onGround)
        {
            WalkingLogic();
        }
        else
        {
            JumpingLogic();
        }
        
        this.gameObject.GetComponent<SpriteRenderer>().sprite = currentSprite;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            moveHorizontal = Input.GetAxis("Horizontal");

            movement = new Vector2(moveHorizontal, 0);

            playerRigidBody2D.velocity = movement * speed * Time.deltaTime;
        }
    }

    IEnumerator WalkingAnimation()
    {
        canChangeSprite = false;

        animationCounter++;

        yield return new WaitForSeconds(.07f);

        canChangeSprite = true;
    }
    IEnumerator JumpingAnimation()
    {
        canChangeSprite = false;

        animationCounter++;

        yield return new WaitForSeconds(.15f);

        canChangeSprite = true;
    }

    IEnumerator PlayerDie()
    {
        dying = true;
        numberOfCoin = 0;

        playerRigidBody2D.velocity = new Vector2(0, 0);
        playerRigidBody2D.gravityScale = 0f;
        playerCollider2D.enabled = false;

        showPlayer.enabled = false;

        yield return new WaitForSeconds(1.5f);

        this.transform.position = originalPosition;

        yield return new WaitForSeconds(.6f);

        showPlayer.enabled = true;
        
        playerCollider2D.enabled = true;
        playerRigidBody2D.gravityScale = 1f;

        health = 1;
        dying = false;
        canMove = true;
    }

    /*
    IEnumerator ReturnToNormalGravity()
    {
        yield return new WaitForSeconds(.3f);

        playerRigidBody2D.gravityScale = 3f;
    }
    */

    IEnumerator PlaySound()
    {
        soundSource.Play();

        yield return new WaitForSeconds(.1f);

        soundPlayed = false;
    }

    private void CheckFacingDirection()
    {
        if(moveHorizontal > 0)
        {
            faceRight = true;
        }
        if(moveHorizontal < 0)
        {
            faceRight = false;
        }
    }
    private void ChangeSpriteBasedOnHealth()
    {
        if (health <= 0)
        {
            canMove = false;
        }
        if (health == 1)
        {
            lowWalkingCounter = 0;
            maxWalkingCounter = 4;
            lowJumpingCounter = 0;
            maxJumpingCounter = 3;
        }
        if (health == 2)
        {
            lowWalkingCounter = 5;
            maxWalkingCounter = 9;
            lowJumpingCounter = 4;
            maxJumpingCounter = 7;
        }
    }
    private void SetAnimationSprite()
    {
        if(faceRight)
        {
            for(int count = lowWalkingCounter; count < maxWalkingCounter; count++)
            {
                walkingSprite[count] = walkingRight[count];
            }
            for (int count = lowJumpingCounter; count < maxJumpingCounter; count++)
            {
                jumpingSprite[count] = jumpingRight[count];
            }
        }
        else
        {
            for (int count = lowWalkingCounter; count < maxWalkingCounter; count++)
            {
                walkingSprite[count] = walkingLeft[count];
            }
            for (int count = lowJumpingCounter; count < maxJumpingCounter; count++)
            {
                jumpingSprite[count] = jumpingLeft[count];
            }
        }
    }

    private void WalkingLogic()
    {
        if (moveHorizontal == 0)
        {
            currentSprite = walkingSprite[lowWalkingCounter];
        }
        else
        {
            if (animationCounter > maxWalkingCounter - 1)
            {
                animationCounter = lowWalkingCounter;
                currentSprite = walkingSprite[animationCounter];
            }
            else
            {
                currentSprite = walkingSprite[animationCounter];
            }
        }

        if (canChangeSprite)
        {
            StartCoroutine(WalkingAnimation());
        }
    }
    private void JumpingLogic()
    {
        if (moveHorizontal == 0)
        {
            currentSprite = jumpingSprite[lowJumpingCounter];
        }
        else
        {
            if (animationCounter > maxJumpingCounter - 1)
            {
                animationCounter = lowJumpingCounter;
                currentSprite = walkingSprite[animationCounter];
            }
            else
            {
                currentSprite = jumpingSprite[animationCounter];
            }
        }

        if (canChangeSprite)
        {
            StartCoroutine(JumpingAnimation());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            onGround = true;
        }
        if (collision.gameObject.tag == "Coin")
        {
            numberOfCoin++;

            soundSource.clip = soundEffect[1];

            if (!soundPlayed)
            {
                soundPlayed = true;
            }
        }
        if (collision.gameObject.tag == "Mushroom")
        {
            health++;

            soundSource.clip = soundEffect[2];

            if (!soundPlayed)
            {
                soundPlayed = true;
            }
        }

        if (collision.gameObject.tag == "KillZone")
        {
            canMove = false;
        }
        if(collision.gameObject.tag == "Enemy")
        {
            health--;
        }
        if(collision.gameObject.tag == "EndGame")
        {
            Application.Quit();
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            nearEnemy = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = false;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            nearEnemy = false;
        }
    }
}
