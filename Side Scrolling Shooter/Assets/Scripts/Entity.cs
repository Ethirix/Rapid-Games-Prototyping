using System;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(PolygonCollider2D))]
public class Entity : MonoBehaviour
{
    public event EventHandler<int> DamageTaken;
    private Health _health;

    protected virtual void Start()
    {
        tag = "Enemy";
        _health = GetComponent<Health>();
        DamageTaken += _health.DealDamage;
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
