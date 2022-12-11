using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject healthbarPrefab;
    [SerializeField] private int health = 100;

    private float _maxHealth;
    private RectTransform _healthBar;
    private float _healthBarWidth;

    public int CurrentHealth { get; private set; }
    public GameObject HealthBarPrefab { get; private set; }
    public bool IsInitialized { get; private set; } = false;

    public EventHandler HealthChanged;

    private void Start()
    {
        HealthBarPrefab = Instantiate(healthbarPrefab, transform);
        HealthBarPrefab.name = "Healthbar";

        _maxHealth = health;
        CurrentHealth = health;
        _healthBar = transform.Find("Healthbar/HealthbarContainer/Background/Health").gameObject.GetComponent<RectTransform>();
        _healthBarWidth = _healthBar.sizeDelta.x;

        IsInitialized = true;
    }
    
    public void DealDamage(int dmg)
    {
        CurrentHealth -= dmg;
        _healthBar.sizeDelta = new Vector2(_healthBarWidth * CurrentHealth / _maxHealth, _healthBar.sizeDelta.y);
        HealthChanged?.Invoke(this, null);
    }

    public void DealDamage(object _, int dmg)
    {
        CurrentHealth -= dmg;
        _healthBar.sizeDelta = new Vector2(_healthBarWidth * CurrentHealth / _maxHealth, _healthBar.sizeDelta.y);
        HealthChanged?.Invoke(this, null);
    }
}
