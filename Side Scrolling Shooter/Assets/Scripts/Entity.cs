using System;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(PolygonCollider2D))]
public class Entity : MonoBehaviour
{
    public event EventHandler<int> DamageTaken;
    private Health _health;

    protected virtual void Start()
    {
        _health = GetComponent<Health>();
        _health.HealthInitialized += OnHealthComponentInitialized;
        DamageTaken += _health.DealDamage;
    }

    protected virtual void OnDestroy()
    {
        DamageTaken -= _health.DealDamage;
    }

    private void OnHealthComponentInitialized(object _, EventArgs __)
    {
        _health.HealthBarPrefab.transform.localPosition = new Vector3(0, GetComponent<PolygonCollider2D>().bounds.extents.y + 0.25f, 0);
        _health.HealthInitialized -= OnHealthComponentInitialized;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            DamageTaken?.Invoke(this, projectile.Damage);
        }
    }
}
