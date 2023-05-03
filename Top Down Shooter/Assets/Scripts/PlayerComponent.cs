using System;
using UnityEngine;

namespace Assets.Scripts
{
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
        }

        private void Reset()
        {
            GameObject healthUI = Resources.Load<GameObject>("Prefabs/HealthUI");
            healthUI = Instantiate(healthUI);
            healthUI.transform.parent = transform;
            healthUI.transform.localPosition = Vector3.zero;
            healthUI.name = "HealthUI";
            healthUI.GetComponent<HealthUIController>().PassHealthInterface(this);
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
            }

            HealthChangedEvent?.Invoke(this, EventArgs.Empty);
        }

    }
}
