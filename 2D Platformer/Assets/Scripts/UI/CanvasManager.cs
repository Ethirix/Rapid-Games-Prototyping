using System;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject restartMenu;

    public event EventHandler RestartGameEvent;

    private GameManager _gameManager;

    private GameObject _instantiatedRestartMenu;

    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
        _gameManager.CheckpointManager.WinEvent += WinEvent;
    }

    private void OnDisable()
    {
        _gameManager.CheckpointManager.WinEvent -= WinEvent;
    }

    private void OnRestartButtonPressedEvent(object sender, EventArgs e)
    {
        _instantiatedRestartMenu.GetComponent<RestartUIBehaviour>().RestartButtonPressedEvent -= OnRestartButtonPressedEvent;
        Destroy(_instantiatedRestartMenu);
        RestartGameEvent?.Invoke(this, null);
    }

    private void WinEvent(object sender, EventArgs e)
    {
        _instantiatedRestartMenu = Instantiate(restartMenu, canvas.transform);
        _instantiatedRestartMenu.GetComponent<RestartUIBehaviour>().RestartButtonPressedEvent += OnRestartButtonPressedEvent;
    }
}
