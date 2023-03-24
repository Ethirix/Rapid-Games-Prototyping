using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    public Transform Transform { get; private set; }
    public event EventHandler<Checkpoint> CheckpointHit;

    private CheckpointManager _checkpointManager;

    private void Awake()
    {
        if (transform.parent.CompareTag("Checkpoint Manager"))
        {
            transform.parent = GameObject.FindGameObjectWithTag("Checkpoint Manager").transform;
        }

        Transform = gameObject.transform;
        tag = "Checkpoint";
    }

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        _checkpointManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<CheckpointManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CheckpointHit?.Invoke(this, this);
        }
    }
}
