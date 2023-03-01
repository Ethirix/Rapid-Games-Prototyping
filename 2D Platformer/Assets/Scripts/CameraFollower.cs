using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] 
    private GameObject target;
    [SerializeField] 
    private float maxDeviation;

    private Vector2 _currentOffset;
    private Rigidbody2D _targetRigidbody2D;

    private float _slerpVal;
    private Vector2 _oldOffset;

    private void Start()
    {
        _targetRigidbody2D = target.GetComponent<Rigidbody2D>();
        transform.position = target.transform.position;
    }

    private void LateUpdate()
    {
        Vector2 targetOffset = _targetRigidbody2D.velocity.normalized * maxDeviation;
        _currentOffset = targetOffset;

        if (_currentOffset != _oldOffset)
        {
            _oldOffset = _currentOffset;
            _slerpVal = 0;
        }

        if (_slerpVal >= 1)
        {
            _slerpVal = 0;
        }

        _slerpVal += Time.deltaTime / 2f;
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, target.transform.position.y, -10) + new Vector3(_currentOffset.x, _currentOffset.y, -10), _slerpVal);
        //TODO: Rework position lerp
        //transform.position = target.transform.position + new Vector3(_currentOffset.x, _currentOffset.y, -10);
    }
}
