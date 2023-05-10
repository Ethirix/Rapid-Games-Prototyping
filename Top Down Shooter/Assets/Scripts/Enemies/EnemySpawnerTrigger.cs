using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

namespace Enemies
{
    public class EnemySpawnerTrigger : MonoBehaviour
    {
        [SerializeField] private Vector2 spawnOffset = new(0, 8f);

        [SerializeField] private List<GameObject> enemiesToSpawn = new();

        private Transform _spawnPoint;
        private bool _triggered;

        private void Start()
        {
            _spawnPoint = transform.Find("SpawnPoint");
            if (_spawnPoint == null)
                Reset();

            _spawnPoint = transform.Find("SpawnPoint");
            _spawnPoint.transform.localPosition = spawnOffset;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || _triggered) return;

            _triggered = true;
            foreach (var enemy in enemiesToSpawn.Select(enemyToSpawn => Instantiate(enemyToSpawn, transform)))
            {
                enemy.transform.localPosition = new Vector3(spawnOffset.x, spawnOffset.y, -1f);
                enemy.transform.parent = null;
            }
        }

        private void Reset()
        {
            GameObject spawn = new("SpawnPoint", typeof(Transform))
            {
                transform =
                {
                    parent = transform,
                    localPosition = new Vector3(spawnOffset.x, spawnOffset.y, 0)
                }
            };
        }
    }
}
