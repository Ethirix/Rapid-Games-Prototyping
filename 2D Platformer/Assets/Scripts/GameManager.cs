using UnityEngine;

[RequireComponent(typeof(CheckpointManager), typeof(PlayerRespawnManager))]
public class GameManager : MonoBehaviour
{
    public CheckpointManager CheckpointManager;
    public PlayerRespawnManager PlayerRespawnManager;

    private void Start()
    {
        CheckpointManager = GetComponent<CheckpointManager>();
        PlayerRespawnManager = GetComponent<PlayerRespawnManager>();
    }
}
