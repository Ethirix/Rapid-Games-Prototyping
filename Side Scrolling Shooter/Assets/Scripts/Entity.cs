using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(PolygonCollider2D), typeof(Rigidbody2D))]
public class Entity : MonoBehaviour
{
    public event EventHandler<int> DamageTaken;
    [SerializeField] private float weaponCooldown = 0.5f;

    protected Health health;
    protected Rigidbody2D rigidbody2D;
    protected readonly List<FiringPoint> firingPoints = new();
    protected GameManager gameManager;
    
    protected virtual void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.freezeRotation = true;
        health = GetComponent<Health>();
        DamageTaken += health.DealDamage;
        health.HealthChanged += OnHealthChanged;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        StartCoroutine(WaitForHealthComponentInitialization());

        foreach (Transform t in gameObject.GetComponentsInChildren<Transform>())
        {
            if (t.CompareTag("FiringPoint"))
            {
                firingPoints.Add(new FiringPoint(t, weaponCooldown));
            }
        }
    }

    private void OnHealthChanged(object sender, EventArgs e)
    {
        if (health.CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        DamageTaken -= health.DealDamage;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            DamageTaken?.Invoke(this, projectile.Damage);
        }
    }

    protected virtual void FixedUpdate()
    {
        MovementLogic();
        FiringLogic();
    }

    protected virtual void MovementLogic() {}

    protected virtual void FiringLogic() {}

    private IEnumerator WaitForHealthComponentInitialization()
    {
        while (true)
        {
            if (health.IsInitialized)
            {
                Vector3 localScale = transform.localScale;
                Vector3 healthScale = health.HealthBarPrefab.transform.localScale;
                health.HealthBarPrefab.transform.localPosition = transform.TransformDirection(new Vector3(0, -GetComponent<PolygonCollider2D>().bounds.extents.y / localScale.y - 0.25f, 0));
                health.HealthBarPrefab.transform.localScale = new Vector3(healthScale.x / localScale.x,
                    healthScale.y / localScale.y, healthScale.z / localScale.z);
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
