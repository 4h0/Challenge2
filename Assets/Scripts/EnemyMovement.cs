using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Sprite[] changeSprite;

    public bool die;

    private SpriteRenderer showEnemy;
    private Collider2D enemyCollider2D;
    private Rigidbody2D enemyRigidBody2D;

    private int changeDirection;

    private void Awake()
    {
        showEnemy = GetComponent<SpriteRenderer>();
        enemyCollider2D = GetComponent<Collider2D>();
        enemyRigidBody2D = GetComponent<Rigidbody2D>();

        die = false;
        changeDirection = 0;
    }

    private void Start()
    {
        StartCoroutine(EnemyMovingAnimation());
    }

    private void Update()
    {
        if (!die)
        {
            if (changeDirection % 2 == 0)
            {
                enemyRigidBody2D.velocity = new Vector2(150, 0) * Time.deltaTime;
            }
            else
            {
                enemyRigidBody2D.velocity = new Vector2(-150, 0) * Time.deltaTime;
            }
        }
        else
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator EnemyMovingAnimation()
    {
        for(int counter = 0; !die; counter++)
        {
            yield return new WaitForSeconds(.1f);

            if (counter % 2 == 0)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = changeSprite[0];
            }
            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = changeSprite[1];
            }
        }
    }

    IEnumerator Die()
    {
        enemyRigidBody2D.velocity = new Vector2 (0,0);

        enemyRigidBody2D.gravityScale = 0f;
        enemyCollider2D.enabled = false;

        this.gameObject.GetComponent<SpriteRenderer>().sprite = changeSprite[2];

        transform.Rotate(new Vector3(0, 0, Random.Range(45f, 180f)) * Time.deltaTime * 3);

        yield return new WaitForSeconds(1.5f);

        showEnemy.enabled = false;

        yield return new WaitForSeconds(.1f);

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "KillZone")
        {
            die = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Mushroom" || collision.gameObject.tag == "Enemy")
        {
            changeDirection++;
        }

        if (collision.gameObject.tag == "KillZone")
        {
            die = true;
        }
    }
}