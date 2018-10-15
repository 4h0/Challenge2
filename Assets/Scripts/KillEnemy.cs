using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemy : MonoBehaviour
{
    private PlayerController playerReference;
    private Collider2D killEnemyCollider2D;

    private Vector3 offsetVector;

    private void Awake()
    {
        playerReference = FindObjectOfType<PlayerController>();

        killEnemyCollider2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        offsetVector = transform.position - playerReference.transform.position;
    }

    private void Update()
    {
        if(playerReference.dying)
        {
            killEnemyCollider2D.enabled = false;
        }
        else
        {
            killEnemyCollider2D.enabled = true;
        }
    }

    private void LateUpdate()
    {
        transform.position = playerReference.transform.position + offsetVector;
    }
}
