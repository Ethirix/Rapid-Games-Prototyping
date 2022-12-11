using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(PolygonCollider2D))]
public class Entity : MonoBehaviour
{
    public event EventHandler<int> DamageTaken;
    [SerializeField] private float weaponCooldown = 0.5f;

    protected Health health;
    protected Rigidbody2D rigidbody2D;
    protected readonly List<FiringPoint> firingPoints = new();
    
    protected virtual void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        DamageTaken += health.DealDamage;
        health.HealthChanged += OnHealthChanged;
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
                health.HealthBarPrefab.transform.localPosition = transform.TransformDirection(new Vector3(0, -GetComponent<PolygonCollider2D>().bounds.extents.y - 0.25f, 0));
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
