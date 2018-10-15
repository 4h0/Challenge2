using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRotating : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(new Vector3(0, Random.Range(45f, 180f), 0) * Time.deltaTime);
    }
}
