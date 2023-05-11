using System;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float force;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void PassDirection(Vector2 direction)
    {
        _direction = direction.normalized;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
            Destroy(other.transform.parent.gameObject);

        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(_direction * force);

        if (_rigidbody.velocity.x > 20)
            _rigidbody.velocity = new Vector2(20, _rigidbody.velocity.y);
        if (_rigidbody.velocity.x < -20)
            _rigidbody.velocity = new Vector2(-20, _rigidbody.velocity.y);
    }
}
