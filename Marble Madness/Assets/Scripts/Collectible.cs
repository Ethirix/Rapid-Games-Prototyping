using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectible : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private float rotationsPerSecond;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController component))
        {
            component.addScore(score);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Quaternion rotation = transform.rotation;
        transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y + 360 * rotationsPerSecond * Time.deltaTime, rotation.eulerAngles.z);
    }
}
