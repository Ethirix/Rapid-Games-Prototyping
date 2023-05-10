using System;
using System.Collections;
using Assets.Scripts;
using UnityEngine;

namespace Enemies
{
    public class EnemyComponent : MonoBehaviour, IEntity
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float damage = 5f;
        [SerializeField] private float damageCooldown = 1f;
        [SerializeField] private Vector2 maxSpeed = new(2.5f, 2.5f);
        [SerializeField] private Sprite deadSprite;

        private bool _canDamage = true;
        private GameObject _player;
        private Rigidbody2D _rigidbody;

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
            _player = GameObject.FindGameObjectWithTag("Player");

            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.freezeRotation = true;
            _rigidbody.gravityScale = 0;
        }

        private void FixedUpdate()
        {
            _rigidbody.AddForce((_player.transform.position - transform.position).normalized * 20f);

            if (_rigidbody.velocity.x > maxSpeed.x)
                _rigidbody.velocity = new Vector2(maxSpeed.x, _rigidbody.velocity.y);
            if (_rigidbody.velocity.x < -maxSpeed.x)
                _rigidbody.velocity = new Vector2(-maxSpeed.x, _rigidbody.velocity.y);

            if (_rigidbody.velocity.y > maxSpeed.y)
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, maxSpeed.y);
            if (_rigidbody.velocity.y < -maxSpeed.y)
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -maxSpeed.y);

        }

        private IEnumerator RunDamageCooldown()
        {
            float time = 0;

            while (time < damageCooldown)
            {
                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            _canDamage = true;
        }

        private IEnumerator RunDead()
        {
            float time = 0;

            while (time < 5)
            {
                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player") && _canDamage)
            {
                _canDamage = false;
                StartCoroutine(RunDamageCooldown());
                other.gameObject.GetComponent<IHealth>().RemoveHealth(damage);
            }
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && _canDamage)
            {
                _canDamage = false;
                StartCoroutine(RunDamageCooldown());
                collision.gameObject.GetComponent<IHealth>().RemoveHealth(damage);
            }
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
                _rigidbody.simulated = false;
                if (deadSprite)
                    GetComponent<SpriteRenderer>().sprite = deadSprite;
                StartCoroutine(RunDead());
            }

            HealthChangedEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}