using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [Tooltip("Speed of the spinning bar")]
    [Range(-100f, 100f)]
    public float rotSpeed = 25;

    void Update()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
    }
}
