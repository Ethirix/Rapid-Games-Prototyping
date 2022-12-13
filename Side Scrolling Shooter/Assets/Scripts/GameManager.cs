using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float belowCameraEnemyKill;
    [SerializeField] private TMP_Text enemiesRemainingText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text winText;
    [SerializeField] private TMP_Text loseText;

    private int score;
    private GameObject _cameraObject;
    private Camera _camera;
    private Spawner _spawner;

    public EventHandler EnemyHit;
    public EventHandler<int> EnemyDead;


    private void Start()
    {
        _camera = Camera.main;
        _cameraObject = Camera.main.gameObject;
        EnemyHit += OnEnemyHit;
        EnemyDead += OnEnemyDead;
        _spawner = FindObjectOfType<Spawner>();
        _spawner.AllWavesComplete += OnWin;
    }

    private void OnDestroy()
    {
        EnemyHit -= OnEnemyHit;
        EnemyDead -= OnEnemyDead;
    }

    private void OnEnemyHit(object sender, EventArgs e)
    {
        score += 10;
    }

    private void OnEnemyDead(object sender, int scr)
    {
        score += scr;
    }

    private void OnWin(object sender, EventArgs e)
    {
        winText.gameObject.SetActive(true);
        StartCoroutine(RestartGame());
    }

    private void FixedUpdate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        enemiesRemainingText.text = $"Enemies Remaining: {enemies.Length}";
        enemies.Where(e =>
                e.transform.position.y <
                _cameraObject.transform.position.y - _camera.orthographicSize - belowCameraEnemyKill).ToList()
            .ForEach(Destroy);

        scoreText.text = $"Score - {score}";

        List<GameObject> players = GameObject.FindGameObjectsWithTag("Player").ToList();
        if (players.Count == 0)
        {
            loseText.gameObject.SetActive(true);
            StartCoroutine(RestartGame());
        }
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
