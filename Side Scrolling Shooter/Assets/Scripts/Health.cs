using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject healthbarPrefab;
    [SerializeField] private int health = 100;

    private float _maxHealth;
    private RectTransform _healthBar;
    private float _healthBarWidth;

    public GameObject HealthBarPrefab { get; private set; }

    public event EventHandler HealthInitialized;

    private void Start()
    {
        HealthBarPrefab = Instantiate(healthbarPrefab, transform);
        HealthBarPrefab.name = "Healthbar";

        _maxHealth = health;
        _healthBar = transform.Find("Healthbar/HealthbarContainer/Background/Health").gameObject.GetComponent<RectTransform>();
        _healthBarWidth = _healthBar.sizeDelta.x;

        HealthInitialized?.Invoke(this, null);
    }
    
    public void DealDamage(int dmg)
    {
        health -= dmg;
        _healthBar.sizeDelta = new Vector2(_healthBarWidth * health / _maxHealth, _healthBar.sizeDelta.y);
    }

    public void DealDamage(object _, int dmg)
    {
        health -= dmg;
        _healthBar.sizeDelta = new Vector2(_healthBarWidth * health / _maxHealth, _healthBar.sizeDelta.y);
    }
}
