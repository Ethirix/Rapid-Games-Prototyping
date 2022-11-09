using UnityEngine;

public class Spin : MonoBehaviour
{
    [Tooltip("Speed of the spinning bar")]
    [Range(-180f, 180f)]
    public float rotSpeed = 25;

    void Update()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
    }
}
