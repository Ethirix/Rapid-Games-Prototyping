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
    [SerializeField]
    private Vector2 minimumVelocityForCameraMovement = new (1, 1);
    [SerializeField]
    private float returnToCentreSpeedPerSecond = 0.5f;


    private Rigidbody2D _playerRigidbody2D;

    private float _timeSinceXChanged;
    private float _timeSinceYChanged;

    private void Start()
    {
        _playerRigidbody2D = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 plrVelocity = _playerRigidbody2D.velocity;
        Vector2 newPos = transform.localPosition;

        _timeSinceXChanged += Time.deltaTime;
        _timeSinceYChanged += Time.deltaTime;

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

        transform.localPosition = new Vector3(newPos.x, newPos.y, -10);
    }
}
