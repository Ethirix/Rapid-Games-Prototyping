using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinUIScript : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnRestartButtonClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit(0);
    }
}
