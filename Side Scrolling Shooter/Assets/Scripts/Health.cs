using UnityEngine;
using UnityEngine.UI;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject healthbarPrefab;
    [SerializeField] private int health = 100;

    private float _maxHealth;
    private RectTransform _healthBar;
    private float _healthBarWidth;

    private void Start()
    {
        Instantiate(healthbarPrefab, transform);
        _maxHealth = health;
        _healthBar = transform.Find("Health").gameObject.GetComponent<RectTransform>();
        _healthBarWidth = _healthBar.sizeDelta.x;
    }
    
    public void DealDamage(int dmg)
    {
        health -= dmg;
        _healthBar.sizeDelta = new(_healthBarWidth * health / _maxHealth, _healthBar.sizeDelta.y);
    }

    public void DealDamage(object _, int dmg)
    {
        health -= dmg;
        _healthBar.sizeDelta = new(_healthBarWidth * health / _maxHealth, _healthBar.sizeDelta.y);
    }
}
