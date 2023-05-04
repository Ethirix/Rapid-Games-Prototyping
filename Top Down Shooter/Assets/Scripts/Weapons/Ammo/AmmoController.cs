using UnityEngine;

namespace Weapons.Ammo
{
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(Rigidbody2D))]
    public class AmmoController : MonoBehaviour
    {
        private AmmoData _data;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;

        public void StartBullet(AmmoData data)
        {
            _data = data;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _data.ProjectileSprite;

            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.gravityScale = 0;
            _rigidbody.freezeRotation = true;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            transform.parent = null;
        }

        private void FixedUpdate()
        {
            _rigidbody.AddRelativeForce(new Vector2(0, _data.ProjectileVelocity), ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out AmmoController _))
                return;

            other.gameObject.TryGetComponent(out IHealth health);
            health?.RemoveHealth(_data.ProjectileDamage);

            Destroy(gameObject);
        }
    }
}
