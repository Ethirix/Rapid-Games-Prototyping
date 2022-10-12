using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public List<Transform> spawnPoints = new List<Transform>();

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

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.sceneCount == 1)
        {
            // load the main scene
            StartCoroutine(loadRequiredScenes());
        }
    }

    public void initialisePlayers()
    {
        GameObject[] playerGOs = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerGo in playerGOs)
        {
            PlayerController playerController = playerGo.GetComponent<PlayerController>();

            if (playerController is not null && playerController.isInitialised == false)
            {
                playerController.init();
            }
        }
    }

    public void setPlayerPosition(PlayerController playerController)
    {
        int playerID = playerController.id;

        if (playerID <= spawnPoints.Count)
        {
            playerController.transform.position = spawnPoints[playerID].position;
        }
    }

    public void setPlayerPositions()
    {
        List<PlayerController> playerControllers = GameManager.Instance.getAllPlayers();

        foreach(PlayerController playerController in playerControllers)
        {
            setPlayerPosition(playerController);
        }
    }

    IEnumerator loadRequiredScenes()
    {
        AsyncOperation mainLoading = SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);

        while (!mainLoading.isDone)
        {
            Debug.Log(mainLoading.progress);

            yield return null;
        }

        // deactivate main scene camera
        GameObject menuCamera = GameObject.FindWithTag("MenuCamera");

        if (menuCamera == true)
        {
            menuCamera.SetActive(false);
        }

        // deactivate main scene light
        GameObject menuLight = GameObject.FindWithTag("MenuLight");

        if (menuLight == true)
        {
            menuLight.SetActive(false);
        }

        GameManager.Instance.setCurrentLevel(SceneManager.GetActiveScene().buildIndex - 1);

        yield return new WaitUntil(() => SceneManager.sceneCount == 3);

        // call the init method on all players
        initialisePlayers();

        GameManager.Instance.playableLevelLoaded();

        UIManager.Instance.setState(UIState.LEVEL);
    }
}
