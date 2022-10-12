using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        ENTERINGSCENE,      // initial state. GameObjects not ticking. Scene not fully initialiased.
        WAITINGTOSTART,     // next state. HandleMatchIsWaitingToStart is called when entering. GameObjects are ticking but players not spawned in. Next state when ReadyToStartMatch returns true or player hits StartMatch
        INPROGRESS,         // HandleMatchHasStarted is called when entering which calls BeginPlay (what, if anything, in Unity?) on all GameObjects. Next state if ReadyToEndMatch returns true or player hits EndMatch
        WAITINGPOSTMATCH,   // HandleMatchHasEnded is called when entering. GameObjects are still ticking but new players not accepted. Transitions when scene transfer starts.
        LEAVINGSCENE,       // last state. HandleLeavingScene is called when entering. Stay in this state while transferring to a new scene
        ABORTEDMATCH        // failure state started from AbortedMatch. Set only if their is an unrecoverable error.
    }

    public static GameManager Instance { get; private set; }

    GameState gameState;
    Scene UI;
    Scene level;

    public Light mainMenuLight;
    public Camera mainMenuCamera;

    Dictionary<int, PlayerController> registeredPlayers = new Dictionary<int, PlayerController>();

    Camera mainCamera;

    bool gameRunning;
    int currentLevel;

    public int pointsPerUnitTravelled = 10;

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
    }

    void Start()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        if (SceneManager.GetSceneByName("UI").isLoaded == false)
        {
            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        }
        else
        {
            UIManager.Instance.setState(UIState.MAINMENU);
        }
    }

    public List<PlayerController> getAllPlayers()
    {
        return registeredPlayers.Values.ToList();
    }

    public bool registerNewPlayer(int id, PlayerController playerController)
    {
        PlayerController registeredPlayer;

        registeredPlayers.TryGetValue(id, out registeredPlayer);

        if (registeredPlayer == null)
        {
            registeredPlayers.Add(id, playerController);

            return true;
        }

        return false;
    }

    public void setCurrentLevel(int level)
    {
        currentLevel = level;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    public void loadNextLevel(bool unloadCurLevel)
    {
        if (unloadCurLevel) // if it's set to true, we unload our current level so we load our new fresh level.
        {
            gameState = GameState.LEAVINGSCENE;
            StartCoroutine(unloadCurrentLevel());
        }

        // have we run out of levels?
        if (currentLevel >= SceneManager.sceneCountInBuildSettings - 2)
        {
            endGame(true);
        }
        else
        {
            currentLevel++;

            StartCoroutine(loadLevel(currentLevel));

            Time.timeScale = 1;
            UIManager.Instance.setState(UIState.LEVEL);
        }
    }

    IEnumerator loadLevel(int currentLevel)
    {
        AsyncOperation currentLevelLoading = SceneManager.LoadSceneAsync("Level " + currentLevel, LoadSceneMode.Additive);

        gameState = GameState.ENTERINGSCENE;

        while (!currentLevelLoading.isDone)
        {
            Debug.Log(currentLevelLoading.progress);

            yield return null;
        }
    }

    public void allowPlayerMovement(bool canMove)
    {
        // set all player controllers movement (on/off)
        foreach (KeyValuePair<int, PlayerController> registeredPlayer in registeredPlayers)
        {
            registeredPlayer.Value.CanMove = canMove;
        }
    }

    public void playableLevelLoaded()
    {
        // enable all lights in level
        GameObject.Find("Lights").SetActive(true);

        // make sure player controllers have been initialised
        LevelManager.Instance.initialisePlayers();

        // set all players to their start points
        LevelManager.Instance.setPlayerPositions();

        // tell all player controllers that they can move
        allowPlayerMovement(true);

        mainCamera = Camera.main;

        Debug.Log("Current Level: " + currentLevel);

        LevelTimer.Instance.reset();

        if (LevelTimer.Instance.IsRunning == false)
        {
            LevelTimer.Instance.start();
        }

        gameState = GameState.INPROGRESS;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main" || scene.name == "UI")
        {
            if (scene.name == "UI")
            {
                if (SceneManager.sceneCount == 2)
                {
                    UIManager.Instance.setState(UIState.MAINMENU);
                }
            }

            return;
        }

        Debug.Log("Scene: " + scene.name + " Loaded!");

        playableLevelLoaded();
    }

    IEnumerator unloadCurrentLevel()
    {
        // disable all lights in scene
        GameObject.Find("Lights").SetActive(false);

        Debug.Log("Unloading Level: " + currentLevel);

        AsyncOperation currentLevelUnloading = SceneManager.UnloadSceneAsync(currentLevel+1);

        while (!currentLevelUnloading.isDone)
        {
            Debug.Log("Unloading progress: " + currentLevelUnloading.progress);

            yield return null;
        }
    }

    public void endGame(bool completed)
    {
        gameState = GameState.WAITINGPOSTMATCH;

        StartCoroutine(endGameSequence(completed));
    }

    IEnumerator endGameSequence(bool completed)
    {
        if (completed == true)
        {
            UIManager.Instance.setState(UIState.COMPLETED);
        }
        else
        {
            UIManager.Instance.setState(UIState.GAMEOVER);
        }

        mainMenuCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(5);


        mainMenuLight.gameObject.SetActive(true);

        unloadCurrentLevel();

        UIManager.Instance.setState(UIState.MAINMENU);
    }

    public void restart()
    {
        Debug.Log("Restarting Level");

        StartCoroutine(unloadCurrentLevel());

        SceneManager.LoadScene(currentLevel, LoadSceneMode.Additive);
        
        Time.timeScale = 1;
        
        UIManager.Instance.setState(UIState.LEVEL);
    }
}
