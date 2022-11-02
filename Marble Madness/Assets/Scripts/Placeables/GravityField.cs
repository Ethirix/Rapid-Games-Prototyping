using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Tooltip("Makes Force To Apply ignore Gravity force")] 
    private bool normalizeGravity;
    [SerializeField, Tooltip("Force to Apply to anything with a Rigidbody")] 
    private float forceToApply;

    [Header("Colours")]
    [SerializeField, Tooltip("Upward Force means anything that overcomes Gravity")] 
    private Color UpwardForceColor;
    [SerializeField, Tooltip("Downward Force means anything that doesn't overcome Gravity")] 
    private Color DownwardForceColor;
    [SerializeField, Tooltip("Neutral Force means anything that is equal to Gravity")] 
    private Color NeutralForceColor;

    private readonly List<GravityCollection> gameObjectsInTrigger = new();
    private float _oldForce;

    private void Start()
    {
        _oldForce = GetForce();
        UpdateColor();
    }

    private float GetForce()
    {
        float force = forceToApply;
        if (normalizeGravity)
        {
            force -= Physics.gravity.y;
        }

        return force;
    }

    private void UpdateColor()
    {
        float force = GetForce();
        force += Physics.gravity.y;
       
        if (force > 0)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_EmissiveColor", UpwardForceColor);
        }
        else if (force < 0)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_EmissiveColor", DownwardForceColor);
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_EmissiveColor", NeutralForceColor);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() is not null)
        {
            gameObjectsInTrigger.Add(new GravityCollection(other.gameObject, other.GetComponent<Rigidbody>()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < gameObjectsInTrigger.Count; i++)
        {
            if (gameObjectsInTrigger[i].GameObject == other.gameObject)
            {
                gameObjectsInTrigger.RemoveAt(i);
            }
        }
    }

    private void FixedUpdate()
    {
        if (GetForce() != _oldForce)
        {
            _oldForce = GetForce();
            UpdateColor();
        }

        foreach (GravityCollection gc in gameObjectsInTrigger)
        {
            float force = 0.0f;
            if (normalizeGravity)
            {
                force -= Physics.gravity.y;
            }
            force += forceToApply;

            gc.Rigidbody.AddForce(new Vector3(0, force, 0), ForceMode.Acceleration);
        }
    }
}

class GravityCollection
{
    public GameObject GameObject;
    public Rigidbody Rigidbody;

    public GravityCollection(GameObject go, Rigidbody rb)
    {
        GameObject = go;
        Rigidbody = rb;
    }
}
