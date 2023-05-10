using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemies
{
    public class EnemySpawnerTrigger : MonoBehaviour
    {
        [SerializeField] private List<GameObject> enemiesToSpawn = new();

        private Transform _spawnPoint;
        private bool _triggered;

        private void Start()
        {
            _spawnPoint = transform.Find("SpawnPoint");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || _triggered) return;

            _triggered = true;
            foreach (GameObject enemy in enemiesToSpawn.Select(enemyToSpawn => Instantiate(enemyToSpawn, transform)))
            {
                Vector3 spawnPos = _spawnPoint.transform.localPosition;
                enemy.transform.localPosition = new Vector3(spawnPos.x, spawnPos.y, -1f);
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
                    localPosition = Vector3.zero
                }
            };
        }
    }
}
