using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Tooltip("The force multiplier of the jump-pad")]
    [Range(1f, 3f)]
    public float jumpForce = 1.5f;
    private float jumpForceMod = 1000f;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            collider.GetComponent<PlayerController>().RB.AddForce(Vector3.up * (jumpForce * jumpForceMod), ForceMode.Force);
        }
    }
}
