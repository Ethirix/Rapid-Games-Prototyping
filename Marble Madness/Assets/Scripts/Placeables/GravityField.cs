using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private readonly List<GravityCollection> _gameObjectsInTrigger = new();
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
        force -= Math.Abs(Physics.gravity.y);
       
        switch (force)
        {
            case > 0:
                gameObject.GetComponent<Renderer>().material.SetColor("_EmissiveColor", UpwardForceColor);
                break;
            case < 0:
                gameObject.GetComponent<Renderer>().material.SetColor("_EmissiveColor", DownwardForceColor);
                break;
            default:
                gameObject.GetComponent<Renderer>().material.SetColor("_EmissiveColor", NeutralForceColor);
                break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() is not null)
        {
            _gameObjectsInTrigger.Add(new GravityCollection(other.gameObject, other.GetComponent<Rigidbody>()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _gameObjectsInTrigger.RemoveAt(_gameObjectsInTrigger.FindIndex(gc => gc.GameObject == other.gameObject));
    }

    private void FixedUpdate()
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (GetForce() != _oldForce)
        {
            _oldForce = GetForce();
            UpdateColor();
        }

        StartCoroutine(RunGravityField());
    }

    private IEnumerator RunGravityField()
    {
        float force = 0.0f;
        if (normalizeGravity)
        {
            force += Math.Abs(Physics.gravity.y);
        }
        force += forceToApply;

        _gameObjectsInTrigger.ForEach(gravityCollection => gravityCollection.Rigidbody.AddForce(new Vector3(0, force, 0), ForceMode.Acceleration));

        yield break;
    }
}

internal class GravityCollection
{
    public readonly GameObject GameObject;
    public readonly Rigidbody Rigidbody;

    public GravityCollection(GameObject go, Rigidbody rb)
    {
        GameObject = go;
        Rigidbody = rb;
    }
}
