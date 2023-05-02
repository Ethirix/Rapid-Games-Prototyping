using UnityEngine;

namespace Weapons.Ammo
{
    public class AmmoController : MonoBehaviour
    {
        private AmmoData _data;
        private Rigidbody2D _rigidbody;

        public void StartBullet(AmmoData data)
        {
            _data = data;
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.gravityScale = 0;
            _rigidbody.freezeRotation = true;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        private void FixedUpdate()
        {
            _rigidbody.AddRelativeForce(new Vector2(0, _data.ProjectileVelocity), ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
            {
                //TODO: Damage Code dependant on HealthController
            }

            if (!other.gameObject.CompareTag("Bullet"))
            {
                Destroy(gameObject);
            }
        }
    }
}
