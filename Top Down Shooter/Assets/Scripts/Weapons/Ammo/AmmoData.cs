using UnityEngine;

namespace Weapons.Ammo
{
    [CreateAssetMenu(fileName = "NewAmmoData", menuName = "Top Down Shooter/Create New Ammo Type")]
    public class AmmoData : ScriptableObject
    {
        [SerializeField, Tooltip("How many Projectiles are shot per 'bullet'")] 
        private byte projectileCount;
        [SerializeField, Tooltip("How fast Projectiles are fired in m/s")] 
        private float projectileVelocity;

        [SerializeField, Tooltip("The Sprite that represents the projectiles shot")] 
        private Sprite projectileSprite;

        [SerializeField, Tooltip("How much damage the projectiles will deal to an entity")]
        private float projectileDamage;

        public byte ProjectileCount => projectileCount;
        public float ProjectileVelocity => projectileVelocity;
        public Sprite ProjectileSprite => projectileSprite;
        public float ProjectileDamage => projectileDamage;
    }
}
