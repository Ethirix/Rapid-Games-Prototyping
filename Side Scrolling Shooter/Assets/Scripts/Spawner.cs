using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private readonly List<GameObject> enemiesToSpawn;
    [SerializeField, Tooltip("If false, will spawn enemies in order of how they're defined in the list")] private bool spawnRandomly;
    [SerializeField, Tooltip("Set to -1 for infinite spawning")] private int spawnCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
