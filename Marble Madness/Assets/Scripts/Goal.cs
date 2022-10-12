using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Goal : MonoBehaviour
{
    public int levelCompleteBonus;
    DecalProjector decalProjector;

    void Awake()
    {
        decalProjector = GetComponent<DecalProjector>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            StartCoroutine(endLevel(playerController));
        }
    }

    IEnumerator endLevel(PlayerController playerController)
    {
        LevelTimer.Instance.stop();

        playerController.addScore(levelCompleteBonus);

        yield return new WaitForSeconds(1);

        GameManager.Instance.allowPlayerMovement(false);

        GameManager.Instance.loadNextLevel(true);
    }
}
