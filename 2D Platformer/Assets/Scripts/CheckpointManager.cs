using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private int initialCheckpoint;
    [SerializeField] private List<Checkpoint> checkpoints;

    private int _checkpointInt;
    private PlayerRespawnManager _respawnManager;

    private void Start()
    {
        _respawnManager = GetComponent<GameManager>().PlayerRespawnManager;

        _checkpointInt = initialCheckpoint;
        _respawnManager.Respawn += OnRespawnPlayer;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            checkpoint.CheckpointHit += OnCheckpointHit;
        }

        _respawnManager.RespawnPlayer();
    }

    private void OnDisable()
    {
        _respawnManager.Respawn -= OnRespawnPlayer;
        
        foreach (Checkpoint checkpoint in checkpoints)
        {
            checkpoint.CheckpointHit -= OnCheckpointHit;
        }
    }

    public void AddCheckpoint(Checkpoint checkpoint)
    {
        if (!checkpoints.Contains(checkpoint))
        {
            return;
        }

        checkpoints.Add(checkpoint);
        checkpoint.CheckpointHit += OnCheckpointHit;
    }

    public bool RemoveCheckpoint(Checkpoint checkpoint)
    {
        int index = checkpoints.IndexOf(checkpoint);
        if (index == -1)
        {
            return false;
        }

        checkpoints.RemoveAt(index);
        return true;
    }

    private void OnRespawnPlayer(object sender, GameObject obj)
    {
        obj.transform.position = new Vector3(checkpoints[_checkpointInt].Transform.position.x,
            checkpoints[_checkpointInt].Transform.position.y + obj.GetComponent<Collider2D>().bounds.extents.y);
    }

    private void OnCheckpointHit(object sender, Checkpoint checkpoint)
    {
        int checkpointInt = checkpoints.IndexOf(checkpoint);
        if (checkpointInt != -1)
        {
            _checkpointInt = checkpointInt;
        }
    }
}
