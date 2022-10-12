using UnityEngine;
using UnityEngine.UI;

public enum UIState
{
    MAINMENU,
    LEVEL,
    COMPLETED,
    GAMEOVER
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    UIState uiState;
    Transform mainMenuUI, levelUI, completedUI, gameOverUI;

    Text time;
    Text scoreP1, scoreP2;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        Transform canvas = GameObject.Find("Canvas").transform;

        mainMenuUI = canvas.Find("Main Menu");
        levelUI = canvas.Find("Level");
        completedUI = canvas.Find("Completed");
        gameOverUI = canvas.Find("Game Over");

        time = levelUI.Find("Timer").GetComponent<Text>();

        scoreP1 = levelUI.Find("ScoreP1").GetComponent<Text>();
        scoreP2 = levelUI.Find("ScoreP2").GetComponent<Text>();

        LevelTimer.Instance.ticked.AddListener(OnLevelTimerTicked);
    }

    void Start()
    {
        uiState = UIState.MAINMENU;
    }

    public void setState(UIState state)
    {
        uiState = state;
        showUI();
    }

    protected void hideUI()
    {
        mainMenuUI.gameObject.SetActive(false);
        levelUI.gameObject.SetActive(false);
        completedUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
    }

    protected void showUI()
    {
        hideUI();

        switch(uiState)
        {
            case UIState.MAINMENU:
                mainMenuUI.gameObject.SetActive(true);
                break;

            case UIState.LEVEL:
                levelUI.gameObject.SetActive(true);
                break;

            case UIState.COMPLETED:
                completedUI.gameObject.SetActive(true);
                break;

            case UIState.GAMEOVER:
                gameOverUI.gameObject.SetActive(true);
                break;
        }
    }

    public void startGame()
    {
        GameManager.Instance.setCurrentLevel(0);
        GameManager.Instance.loadNextLevel(false);
    }

    public void updateScore(int id, int score)
    {
        switch (id)
        {
            case 0:
                scoreP1.text = score.ToString();
                break;

            case 1:
                scoreP2.text = score.ToString();
                break;
        }
    }

    public void OnLevelTimerTicked(int remainingTime)
    {
        time.text = remainingTime.ToString();
    }
}
