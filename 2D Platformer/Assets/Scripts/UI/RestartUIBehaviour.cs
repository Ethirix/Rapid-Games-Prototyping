using System;
using UnityEngine;
using UnityEngine.UI;

public class RestartUIBehaviour : MonoBehaviour
{
    [SerializeField] private Button quitButton;
    [SerializeField] private Button restartButton;

    public event EventHandler RestartButtonPressedEvent;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        quitButton.onClick.AddListener(QuitButtonClicked);
        restartButton.onClick.AddListener(RestartButtonClicked);
    }

    private void OnDisable()
    {
        quitButton.onClick.RemoveListener(QuitButtonClicked);
        restartButton.onClick.RemoveListener(RestartButtonClicked);
    }

    private void QuitButtonClicked()
    {
        Application.Quit(0);
    }

    private void RestartButtonClicked()
    {
        RestartButtonPressedEvent?.Invoke(this, null);
    }
}
