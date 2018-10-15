using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushRoomBrick : MonoBehaviour
{
    public GameObject coinReference;

    public Sprite changeSprite;

    private Collider2D mushroomCollider2D;

    private bool playerHit;
    private bool spriteChanged;

    private void Awake()
    {
        mushroomCollider2D = GetComponent<Collider2D>();

        playerHit = false;
    }

    private void Update()
    {
        if (!playerHit)
        {
            coinReference.SetActive(false);
        }
        else
        {
            if (!spriteChanged)
            {
                StartCoroutine(ChangeSprite());
            }
        }
    }

    IEnumerator ChangeSprite()
    {
        spriteChanged = true;

        coinReference.SetActive(true);
        mushroomCollider2D.enabled = false;

        this.gameObject.GetComponent<SpriteRenderer>().sprite = changeSprite;

        yield return new WaitForEndOfFrame();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHit = true;
        }
    }
}
