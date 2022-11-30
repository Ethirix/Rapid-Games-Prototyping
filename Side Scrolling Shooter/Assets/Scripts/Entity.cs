using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Entity : MonoBehaviour
{
    public event EventHandler<int> DamageTaken;

    protected virtual void Start()
    {
        tag = "Enemy";
    }
}
