using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField]
        private List<WeaponBase> weapons = new();
       
        private int _weapon = -1;

        private void Awake()
        {
            if (weapons.Count > 0)
            {
                _weapon = 0;
            }
        }

        public void Shoot(Transform shotRotation, Transform shotPosition)
        {
            if (_weapon == -1)
                return;

            weapons[_weapon].Shoot(shotRotation, shotPosition);
        }

        public void StopShooting()
        {
            if (_weapon == -1)
                return;

            weapons[_weapon].EndShooting();
        }

        public void Reload()
        {
            if (_weapon == -1)
                return;

            weapons[_weapon].Reload();
        }

        public void AddWeapon(WeaponBase weapon, bool equip = false)
        {
            weapons.Add(weapon);

            if (equip)
            {
                _weapon = weapons.IndexOf(weapon);
            }
        }

        public bool RemoveWeapon(WeaponBase weapon)
        {
            if (weapons.IndexOf(weapon) == _weapon)
            {
                _weapon = -1;
            }

            return weapons.Remove(weapon);
        }

        public bool EquipWeapon(WeaponBase weapon)
        {
            if (weapons.Contains(weapon))
            {
                _weapon = weapons.IndexOf(weapon);
                return true;
            }

            return false;
        }

        public bool EquipWeapon(int weapon)
        {
            if (weapon < weapons.Count)
            {
                weapons[_weapon].EndShooting();
                _weapon = weapon;
                return true;
            }

            return false;
        }

        public WeaponBase GetCurrentWeapon()
        {
            return weapons[_weapon];
        }

        public int GetCurrentWeaponInt()
        {
            return _weapon;
        }

        public int GetWeaponsSize()
        {
            return weapons.Count;
        }
    }
}
