using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Text coinText;

    private PlayerController playerControllerReference;

    private void Awake()
    {
        playerControllerReference = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        coinText.text = "Player Coins: " + playerControllerReference.numberOfCoin;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
