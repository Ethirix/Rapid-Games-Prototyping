using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject healthbarPrefab;
    [SerializeField] private int health;

    private float _maxHealth;
    private GameObject _healthBar;
    private GameObject parentObject;

    private void Start()
    {
        


        _maxHealth = health;
        _healthBar = transform.Find("Health").gameObject;
    }

    
}
