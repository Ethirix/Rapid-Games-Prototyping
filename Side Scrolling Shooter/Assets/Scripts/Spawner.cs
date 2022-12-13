using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<Wave> waves;
    private int _waveNumber;
    private bool _allWavesSpawned = false;

    public EventHandler AllWavesComplete;

    private void Start()
    {
        StartCoroutine(RunWaveSpawning());
    }

    private void FixedUpdate()
    {
        if (_allWavesSpawned && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            AllWavesComplete?.Invoke(this, null);
        }
    }

    private IEnumerator RunWaveSpawning()
    {
        while (_waveNumber < waves.Count)
        {
            int enemyToSpawn = 0;
            int leftCount = 0;
            int rightCount = 0;
            for (int i = 0; i < waves[_waveNumber].NumberEnemiesToSpawn; i++)
            {
                if (waves[_waveNumber].NumberEnemiesToSpawn == 0)
                {
                    continue;
                }

                GameObject enemy = Instantiate(waves[_waveNumber].EnemiesToSpawn[enemyToSpawn]);

                switch (i % 2)
                {
                    case 0:
                        enemy.transform.position = new Vector3(
                            leftCount * enemy.GetComponent<PolygonCollider2D>().bounds.size.x +
                            waves[_waveNumber].EnemySpawnSpread * leftCount + waves[_waveNumber].EnemySpawnSpread, transform.position.y);
                        leftCount++;
                        break;
                    case 1:
                        enemy.transform.position = new Vector3(
                            -(rightCount * enemy.GetComponent<PolygonCollider2D>().bounds.size.x +
                            waves[_waveNumber].EnemySpawnSpread * rightCount + waves[_waveNumber].EnemySpawnSpread), transform.position.y);
                        rightCount++; 
                        break;
                }

                enemyToSpawn++;
                if (enemyToSpawn >= waves[_waveNumber].EnemiesToSpawn.Count)
                {
                    enemyToSpawn = 0;
                }

            }

            yield return new WaitForSeconds(waves[_waveNumber].DelayUntilNextWave);
            _waveNumber++;
        }

        _allWavesSpawned = true;
    }
}
