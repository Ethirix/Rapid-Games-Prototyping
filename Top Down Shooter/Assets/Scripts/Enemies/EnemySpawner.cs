using System;
using UnityEngine;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        private EnemySpawnerTrigger _trigger;

        private void Start()
        {
            Transform trigger = transform.Find("Trigger");
            if (trigger == null)
                Reset();

            _trigger = trigger.GetComponent<EnemySpawnerTrigger>();
        }

        private void Reset()
        {
            GameObject _ = new("Trigger", typeof(BoxCollider2D))
            {
                transform = { parent = transform }
            };
        }
    }
}
