using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HealthUIController : MonoBehaviour
    {
        private Slider _healthSlider;
        private IHealth _health;

        private void Start()
        {
            _healthSlider = transform.Find("Canvas").Find("Slider").GetComponent<Slider>();
        }

        public void PassHealthInterface(IHealth health)
        {
            _health = health;
            _health.HealthChangedEvent += OnHealthChangedEvent;
        }

        private void OnHealthChangedEvent(object sender, EventArgs e)
        {
            _healthSlider.value = _health.Health / _health.MaxHealth;
        }

    }
}
