using System.Collections;
using UnityEngine;
using Weapons.Ammo;

namespace Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [SerializeField] protected WeaponData weaponData;
        [SerializeField] protected AmmoData ammoData;

        protected virtual void Awake()
        {
            magazineAmmoCount = weaponData.MagazineSize;
        }

        protected bool blockShooting;
        protected bool blockReloading;
        protected bool currentlyShooting;
        protected byte magazineAmmoCount;
        protected bool stopShooting;

        public byte GetMagazineAmmo() => magazineAmmoCount;

        public abstract void Shoot(Transform shotRotation, Transform shotPosition);

        public abstract void EndShooting();

        public abstract void Reload();

        protected abstract IEnumerator RunShooting(Transform shotRotation, Transform shotPosition);

        protected abstract IEnumerator RunShotCooldown();

        protected abstract IEnumerator RunReload();
    }
}
