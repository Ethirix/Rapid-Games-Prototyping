using System;
using System.Collections;
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

    private readonly List<GravityCollection> _gameObjectsInTrigger = new();
    private float _oldForce;

    private void Start()
    {
        _oldForce = GetForce();
        UpdateColor();
    }

    /// <summary>
    /// Calculates the Force that will be applied to a <c>Rigidbody</c>
    /// </summary>
    /// <returns>The calculated force</returns>
    private float GetForce()
    {
        float force = forceToApply;
        if (normalizeGravity)
        {
            force -= Physics.gravity.y;
        }

        return force;
    }

    /// <summary>
    /// Changes the Color of the GravityField based on force Calculated in <c>GetForce()</c>
    /// </summary>
    private void UpdateColor()
    {
        float force = GetForce();
        force -= Math.Abs(Physics.gravity.y);

        Color color = force switch
        {
            > 0 => UpwardForceColor,
            < 0 => DownwardForceColor,
            _ => NeutralForceColor
        };

        gameObject.GetComponent<Renderer>().material.SetColor("_EmissiveColor", color);
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
        //Finds the index of the matching GravityCollection.GameObject to then remove it from the List
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

    /// <summary>
    /// Applies the Force to all <c>GameObjects</c> within the Gravity Field
    /// </summary>
    private IEnumerator RunGravityField()
    {
        float force = 0.0f;
        if (normalizeGravity)
        {
            force += Math.Abs(Physics.gravity.y);
        }
        force += forceToApply;

        //Uses List.ForEach to apply the force to each Rigidbody
        _gameObjectsInTrigger.ForEach(gravityCollection => gravityCollection.Rigidbody.AddForce(new Vector3(0, force, 0), ForceMode.Acceleration));

        yield break;
    }
}

/// <summary>
/// Holds references to a <c>GameObject</c> and <c>Rigidbody</c> to avoid many <c>GetComponent()</c> calls
/// </summary>
internal class GravityCollection
{
    public readonly GameObject GameObject;
    public readonly Rigidbody Rigidbody;

    /// <summary>
    /// Constructs the <c>GravityCollection</c> Object
    /// </summary>
    /// <param name="go"><c>GameObject</c> reference</param>
    /// <param name="rb"><c>Rigidbody</c> reference to reduce <c>GetComponent</c> calls</param>
    public GravityCollection(GameObject go, Rigidbody rb)
    {
        GameObject = go;
        Rigidbody = rb;
    }
}
