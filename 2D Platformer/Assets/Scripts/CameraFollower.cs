using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] 
    private GameObject target;

    private void LateUpdate()
    {
        transform.position = target.transform.position;
    }
}
