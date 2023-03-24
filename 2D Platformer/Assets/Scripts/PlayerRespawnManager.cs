using System;
using UnityEngine;

public class PlayerRespawnManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float PlayerKillHeight = -50f;

    public event EventHandler<GameObject> Respawn;

    private void FixedUpdate()
    {
        if (player.transform.position.y <= PlayerKillHeight)
        {
            RespawnPlayer();
        }
    }

    public void RespawnPlayer()
    {
        Respawn?.Invoke(this, player);
    }
}
