using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomMove : MonoBehaviour
{
    public bool die;
    
    private Rigidbody2D mushRigidBody2D;

    private int changeDirection;

    private void Start()
    {
        mushRigidBody2D = GetComponent<Rigidbody2D>();

        die = false;
        changeDirection = 0;
    }

    private void Update()
    {
        if (!die)
        {
            if (changeDirection % 2 == 0)
            {
                mushRigidBody2D.velocity = new Vector2(150, 0) * Time.deltaTime;
            }
            else
            {

                mushRigidBody2D.velocity = new Vector2(-150, 0) * Time.deltaTime;
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "KillZone")
        {
            die = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            die = true;
        }

        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Enemy")
        {
            changeDirection++;
        }

        if (collision.gameObject.tag == "KillZone")
        {
            die = true;
        }
    }
}
