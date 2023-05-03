using System;

namespace Assets.Scripts
{
    public interface IHealth
    {
        public EventHandler HealthChangedEvent { get; set; }

        public float MaxHealth { get; }
     
        public float Health { get; }
     
        public bool IsAlive { get; }

        public void AddHealth(float health);
        public void RemoveHealth(float health);
    }
}