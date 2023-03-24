using System;
using UnityEngine;

public class PlayerRespawnManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float playerKillHeight = -50f;

    public event EventHandler<GameObject> Respawn;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
        _gameManager.CheckpointManager.RestartPlayerEvent += ResetPlayer;
    }

    private void FixedUpdate()
    {
        if (player.transform.position.y <= playerKillHeight)
        {
            RespawnPlayer();
        }
    }

    private void ResetPlayer(object sender, EventArgs e)
    {
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        Respawn?.Invoke(this, player);
    }
}
