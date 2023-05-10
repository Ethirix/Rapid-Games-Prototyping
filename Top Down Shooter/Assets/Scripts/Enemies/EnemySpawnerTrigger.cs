using System;
using UnityEngine;

namespace Enemies
{
    public class EnemySpawnerTrigger : MonoBehaviour
    {
        private Transform _spawnPoint;
        private bool _triggered;

        private void Start()
        {
            _spawnPoint = transform.Find("SpawnPoint");
            if (_spawnPoint == null)
                Reset();

            _spawnPoint = transform.Find("SpawnPoint");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !_triggered)
            {
                _triggered = true;
                new GameObject("lol", typeof(Transform)).transform.position = _spawnPoint.position;
            }
        }

        private void Reset()
        {
            GameObject spawn = new("SpawnPoint", typeof(Transform))
            {
                transform =
                {
                    parent = transform,
                    localPosition = new Vector3(0, 8, 0)
                }
            };
        }
    }
}
