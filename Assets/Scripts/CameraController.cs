using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController playerReference;

    private Vector3 offsetVector;
    private Vector3 playerOriginalPosition;
    private Vector3 cameraOriginalPosition;

    private void Awake()
    {
        playerReference = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        offsetVector = transform.position - playerReference.transform.position;

        playerOriginalPosition = new Vector3(0, playerReference.transform.position.y, 0);
        cameraOriginalPosition = this.transform.position;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(playerReference.transform.position.x + offsetVector.x, 0f, cameraOriginalPosition.z);
    }
}
