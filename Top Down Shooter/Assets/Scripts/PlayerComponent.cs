using System;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerComponent : MonoBehaviour, IEntity
{
    [SerializeField] private float maxHealth = 100f;

    private void Awake()
    {
        MaxHealth = maxHealth;
        Health = MaxHealth;
    }

    private void Start()
    {
        if (transform.Find("HealthUI") == null)
        {
            Reset();
        }

        GetComponentInChildren<HealthUIController>().PassHealthInterface(this);
    }

    private void Reset()
    {
        GameObject healthUI = Resources.Load<GameObject>("Prefabs/HealthUI");
        healthUI = Instantiate(healthUI, transform, true);
        healthUI.transform.localPosition = Vector3.zero;
        healthUI.name = "HealthUI";
    }

    public EventHandler HealthChangedEvent { get; set; }

    public float MaxHealth { get; private set; }
    public float Health { get; private set; }
    public bool IsAlive { get; private set; } = true;

    public void AddHealth(float health)
    {
        Health += health;
        if (Health > MaxHealth)
            Health = MaxHealth;

        HealthChangedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveHealth(float health)
    {
        Health -= health;
        if (Health <= 0)
        {
            Health = 0;
            IsAlive = false;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        HealthChangedEvent?.Invoke(this, EventArgs.Empty);
    }
}