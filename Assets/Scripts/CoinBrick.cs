using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CoinBrick : MonoBehaviour
{
    public GameObject coinReference;

    public Sprite changeSprite;

    private Collider2D brickCollider2D;

    private bool playerHit;
    private bool spriteChanged;
    
    private void Awake()
    {
        brickCollider2D = GetComponent<Collider2D>();

        playerHit = false;
    }

    private void Update()
    {
        if(!playerHit)
        {
            coinReference.SetActive(false);
        }
        else
        {
            if(!spriteChanged)
            {
                StartCoroutine(ChangeSprite());
            }
        }
    }

    IEnumerator ChangeSprite()
    {
        spriteChanged = true;

        coinReference.SetActive(true);
        brickCollider2D.enabled = false;

        this.gameObject.GetComponent<SpriteRenderer>().sprite = changeSprite;

        for(int counter = 0; counter < 16; counter++)
        {
            if(counter % 2 == 0)
            {
                yield return new WaitForSeconds(.1f);

                coinReference.SetActive(true);
            }
            else
            {
                yield return new WaitForSeconds(.1f);

                coinReference.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerHit = true;
        }
    }
}
