using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class KillVolume : MonoBehaviour
{    
    protected const string playerTag = "Player";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            playerController.RB.velocity = Vector3.zero;
            playerController.RB.angularVelocity = Vector3.zero;

            LevelManager.Instance.setPlayerPosition(playerController);
        }
    }
}
