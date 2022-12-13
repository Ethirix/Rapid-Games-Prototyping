using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
    [SerializeField] private int delayUntilNextWave;
    [SerializeField] private float enemySpawnSpread;
    [SerializeField] private int numberEnemiesToSpawn = 3;

    public int DelayUntilNextWave => delayUntilNextWave;
    public float EnemySpawnSpread => enemySpawnSpread;
    public int NumberEnemiesToSpawn => numberEnemiesToSpawn;

    public List<GameObject> EnemiesToSpawn = new();
}