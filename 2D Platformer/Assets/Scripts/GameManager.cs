using System;
using UnityEngine;

[RequireComponent(typeof(CheckpointManager), typeof(PlayerRespawnManager))]
public class GameManager : MonoBehaviour
{
    public CheckpointManager CheckpointManager;
    public PlayerRespawnManager PlayerRespawnManager;
    public CanvasManager CanvasManager;

    public GameState GameState { get; private set; } = GameState.None;

    private void Start()
    {
        GameState = GameState.Loading;
        CheckpointManager = GetComponent<CheckpointManager>();
        PlayerRespawnManager = GetComponent<PlayerRespawnManager>();
        CanvasManager = GetComponent<CanvasManager>();
        CheckpointManager.WinEvent += WinEvent;
        CanvasManager.RestartGameEvent += RestartGame;

        GameState = GameState.Playing;
    }

    private void OnDisable()
    {
        CheckpointManager.WinEvent -= WinEvent;
    }

    private void WinEvent(object sender, EventArgs e)
    {
        Debug.Log("Game Won");
        GameState = GameState.Won;
        Time.timeScale = 0;
    }

    public void RestartGame(object sender, EventArgs e)
    {
        GameState = GameState.Playing;
        Time.timeScale = 1;
    }
}

public enum GameState
{
    None,
    Loading,
    Playing,
    Won
}
