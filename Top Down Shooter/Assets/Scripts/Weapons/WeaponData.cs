using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Top Down Shooter/Create New Weapon Type")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField, Tooltip("How many bullets does the magazine carry.")] 
        private byte magazineSize = 10;
        [SerializeField, Tooltip("Time it takes to reload the weapon.")] 
        private float reloadTime = 1f;

        [SerializeField, Tooltip("Radius that bullets will stay within at 5m range.")] 
        private float spreadRadius = 0.5f;
        [SerializeField, Tooltip("Time between each shot.")]
        private float shotDelay = 0.05f;

        [SerializeField, Tooltip("How many times the gun shoots per trigger pull.")]
        private byte roundsPerTriggerPull;

        public byte MagazineSize => magazineSize;
        public byte RoundsPerTriggerPull => roundsPerTriggerPull;
        public float SpreadRadius => spreadRadius;
        public float ReloadTime => reloadTime;
        public float ShotDelay => shotDelay;
    }
}
