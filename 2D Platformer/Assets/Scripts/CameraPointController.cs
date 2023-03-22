using UnityEngine;

public class CameraPointController : MonoBehaviour
{
    [SerializeField] 
    private GameObject player;
    [SerializeField] 
    private Vector2 offsetBounds = Vector2.zero;
    [SerializeField] 
    private float offsetPerSecond = 1.5f;
    [SerializeField] 
    private Vector2 delayToReturnToCentre = new (1, 1);
    [SerializeField, Space]
    private Vector2 minimumVelocityForCameraMovement = new (1, 1);
    [SerializeField]
    private float returnToCentreSpeedPerSecond = 0.5f;
    [SerializeField, Space] 
    private Vector2 cameraSizeBounds = new(7.5f, 15f);
    [SerializeField] 
    private float cameraSizeOffsetPerSecond = 1.0f;
    [SerializeField] 
    private float minimumVelocityForCameraSize = 1.0f;
    [SerializeField] 
    private float delayToRevertCameraSize = 1.0f;
    [SerializeField] 
    private float returnToDefaultCameraSizePerSecond = 1.0f;



    private Rigidbody2D _playerRigidbody2D;

    private float _timeSinceXChanged;
    private float _timeSinceYChanged;
    private float _timeSinceCameraSizeChanged;

    private void Start()
    {
        _playerRigidbody2D = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 plrVelocity = _playerRigidbody2D.velocity;
        Vector3 newPos = transform.localPosition;

        _timeSinceXChanged += Time.deltaTime;
        _timeSinceYChanged += Time.deltaTime;
        _timeSinceCameraSizeChanged += Time.deltaTime;

        if (plrVelocity.x > minimumVelocityForCameraMovement.x)
        {
            newPos.x += Mathf.Sin(offsetPerSecond * Time.deltaTime) * offsetBounds.x;
            _timeSinceXChanged = 0;
        }
        if (plrVelocity.y > minimumVelocityForCameraMovement.y)
        {
            newPos.y += Mathf.Sin(offsetPerSecond * Time.deltaTime) * offsetBounds.y;
            _timeSinceYChanged = 0;
        }
        if (plrVelocity.x < -minimumVelocityForCameraMovement.x)
        {
            newPos.x -= Mathf.Sin(offsetPerSecond * Time.deltaTime) * offsetBounds.x;
            _timeSinceXChanged = 0;
        }
        if (plrVelocity.y < -minimumVelocityForCameraMovement.y)
        {
            newPos.y -= Mathf.Sin(offsetPerSecond * Time.deltaTime) * offsetBounds.y;
            _timeSinceYChanged = 0;
        }

        if (plrVelocity.x > minimumVelocityForCameraSize || plrVelocity.x < -minimumVelocityForCameraSize ||
            plrVelocity.y > minimumVelocityForCameraSize || plrVelocity.y < -minimumVelocityForCameraSize)
        {
            newPos.z += Mathf.Sin(cameraSizeOffsetPerSecond * Time.deltaTime) * cameraSizeBounds.y;
            _timeSinceCameraSizeChanged = 0;
        }

        if (newPos.x > offsetBounds.x)
        {
            newPos.x = offsetBounds.x;
        }
        if (newPos.x < -offsetBounds.x)
        {
            newPos.x = -offsetBounds.x;
        }
        if (newPos.y > offsetBounds.y)
        {
            newPos.y = offsetBounds.y;
        }
        if (newPos.y < -offsetBounds.y)
        {
            newPos.y = -offsetBounds.y;
        }
        if (newPos.z > cameraSizeBounds.y)
        {
            newPos.z = cameraSizeBounds.y;
        }
        if (newPos.z < cameraSizeBounds.x)
        {
            newPos.z = cameraSizeBounds.x;
        }

        if (plrVelocity.x == 0)
        {
            switch (newPos.x)
            {
                case > 0 when _timeSinceXChanged > delayToReturnToCentre.x:
                    newPos.x -= Mathf.Sin(returnToCentreSpeedPerSecond * Time.deltaTime) * offsetBounds.x;
                    break;
                case < 0 when _timeSinceXChanged > delayToReturnToCentre.x:
                    newPos.x += Mathf.Sin(returnToCentreSpeedPerSecond * Time.deltaTime) * offsetBounds.x;
                    break;
            }
        }

        if (plrVelocity.y == 0)
        {
            switch (newPos.y)
            {
                case > 0 when _timeSinceYChanged > delayToReturnToCentre.y:
                    newPos.y -= Mathf.Sin(returnToCentreSpeedPerSecond * Time.deltaTime) * offsetBounds.y;
                    break;
                case < 0 when _timeSinceYChanged > delayToReturnToCentre.y:
                    newPos.y += Mathf.Sin(returnToCentreSpeedPerSecond * Time.deltaTime) * offsetBounds.y;
                    break;
            }
        }

        if (plrVelocity.x == 0 && plrVelocity.y == 0 && _timeSinceCameraSizeChanged > delayToRevertCameraSize)
        {
            newPos.z -= Mathf.Sin(returnToDefaultCameraSizePerSecond * Time.deltaTime) * cameraSizeBounds.x;
        }

        if (newPos.x is <= 0.001f and >= -0.001f)
        {
            newPos.x = 0;
        }
        if (newPos.y is <= 0.001f and >= -0.001f)
        {
            newPos.y = 0;
        }
        if (newPos.z <= cameraSizeBounds.x + 0.001f)
        {
            newPos.z = cameraSizeBounds.x;
        }

        transform.localPosition = newPos;
    }
}
