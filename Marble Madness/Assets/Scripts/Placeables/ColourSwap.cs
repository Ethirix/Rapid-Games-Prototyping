using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourSwap : MonoBehaviour
{
    [Tooltip("Material the ball will be swapped to")]
    public Material targetMaterial;

    void Start()
    {
        gameObject.transform.Find("ColourIndicator").GetComponentInChildren<Renderer>().material = targetMaterial;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Renderer>().material = targetMaterial;
        }
    }
}