using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private int initialCheckpoint;
    [SerializeField] private int winCheckpoint;
    [SerializeField] private List<Checkpoint> checkpoints;

    public event EventHandler<Checkpoint> CheckpointChangedEvent;
    public event EventHandler WinEvent;
    public event EventHandler RestartPlayerEvent;

    private int _checkpointInt;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GetComponent<GameManager>();

        _checkpointInt = initialCheckpoint;
        _gameManager.PlayerRespawnManager.Respawn += OnRespawnPlayer;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            checkpoint.CheckpointHit += OnCheckpointHit;
        }

        _gameManager.PlayerRespawnManager.RespawnPlayer();
        _gameManager.CanvasManager.RestartGameEvent += OnRestartGameEvent;
    }

    private void OnDisable()
    {
        _gameManager.PlayerRespawnManager.Respawn -= OnRespawnPlayer;
        _gameManager.CanvasManager.RestartGameEvent -= OnRestartGameEvent;
        
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
        if (checkpointInt == -1) return;

        _checkpointInt = checkpointInt;
        CheckpointChangedEvent?.Invoke(this, checkpoints[_checkpointInt]);

        if (checkpointInt == winCheckpoint)
        {
            WinEvent?.Invoke(this, null);
        }
    }

    public void OnRestartGameEvent(object sender, EventArgs e)
    {
        _checkpointInt = initialCheckpoint;
        CheckpointChangedEvent?.Invoke(this, checkpoints[_checkpointInt]);
        RestartPlayerEvent?.Invoke(this, null);
    }

    public bool IsCheckpointActive(Checkpoint checkpoint) => checkpoints.IndexOf(checkpoint) == _checkpointInt;
}
