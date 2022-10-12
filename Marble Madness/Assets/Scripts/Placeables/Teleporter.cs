using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Tooltip("Transform that the player will be teleported to")]
    public Transform targetLocation;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.position = targetLocation.position;
        }
    }
}