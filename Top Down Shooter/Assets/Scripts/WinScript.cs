using UnityEngine;

public class WinScript : MonoBehaviour
{
    [SerializeField] private GameObject winUIPrefab;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(winUIPrefab);
            Time.timeScale = 0f;
        }
    }
}
