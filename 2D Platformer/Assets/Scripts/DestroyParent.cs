using UnityEngine;

public class DestroyParent : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            Destroy(transform.parent.parent.gameObject);
    }
}
