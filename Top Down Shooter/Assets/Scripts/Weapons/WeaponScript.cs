using System.Collections;
using UnityEngine;
using Weapons.Ammo;

namespace Weapons
{
    public class WeaponScript : WeaponBase
    {
        public override void Shoot(Transform shotRotation, Transform shotPosition)
        {
            if (blockShooting || magazineAmmoCount <= 0)
                return;

            StartCoroutine(RunShooting(shotRotation, shotPosition));
        }

        public override void EndShooting()
        {
            if (currentlyShooting)
                stopShooting = true;
        }

        public override void Reload()
        {
            if (blockReloading)
                return;

            StartCoroutine(RunReload());
        }

        protected override IEnumerator RunShooting(Transform shotRotation, Transform shotPosition)
        {
            blockShooting = true;
            currentlyShooting = true;

            while (!stopShooting)
            {
                //TODO: Figure out Cone maths when applying to shotDirection.

                if (magazineAmmoCount <= 0 || blockReloading)
                    break;
                

                if (weaponData.RoundsPerTriggerPull <= 0)
                {
                    StartCoroutine(RunShoot(shotRotation, shotPosition));
                    yield return RunShotCooldown();
                }
                else
                {
                    for (int i = 0; i < weaponData.RoundsPerTriggerPull; i++)
                    {
                        if (magazineAmmoCount == 0)
                            break;
                        StartCoroutine(RunShoot(shotRotation, shotPosition));
                        yield return RunShotCooldown();
                    }

                    stopShooting = true;
                }
            }

            currentlyShooting = false;
            stopShooting = false;
            blockShooting = false;
        }

        private IEnumerator RunShoot(Transform shotRotation, Transform shotPosition)
        {
            magazineAmmoCount--;

            for (int i = 0; i < ammoData.ProjectileCount; i++)
            {
                GameObject bullet = new()
                {
                    transform = { parent = shotPosition },
                    name = "Bullet"
                };
                bullet.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, shotRotation.rotation.eulerAngles.z));
                bullet.transform.localPosition = new Vector3(0, 1.1f, -1);
                bullet.AddComponent<AmmoController>().StartBullet(ammoData);
            }

            yield return null;
        }

        protected override IEnumerator RunShotCooldown()
        {
            float time = 0;
            while (time <= weaponData.ShotDelay)
            {
                yield return new WaitForFixedUpdate();
                time += Time.fixedDeltaTime;
            }
        }

        protected override IEnumerator RunReload()
        {
            blockReloading = true;
            blockShooting = true;

            float time = 0;
            while (time <= weaponData.ReloadTime)
            {
                yield return new WaitForFixedUpdate();
                time += Time.fixedDeltaTime;
            }

            magazineAmmoCount = weaponData.MagazineSize;

            blockReloading = false;
            blockShooting = false;
        }
    }
}