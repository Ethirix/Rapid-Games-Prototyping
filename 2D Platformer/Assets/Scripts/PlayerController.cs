using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D)), SelectionBase]
public class PlayerController : MonoBehaviour, Controls.IGameActions
{
    [Header("Base Settings")]
    [SerializeField] 
    private float maxMovementSpeed;
    [SerializeField]
    private float acceleration;

    [Header("Jump Settings")]
    [SerializeField, Range(1, 10)]
    private int maximumJumpCount;
    [SerializeField] 
    private float jumpForce;

    [Header("Floor Detection Settings")]
    [SerializeField]
    private float raycastDistance;
    [SerializeField, Tooltip("From the base of the sprite, in World units, the starting position of the Raycast for the floor.")] 
    private float raycastStartPosition;

    private int _jumpCount;

    private Collider2D _collider;
    private Bounds _bounds;
    private Rigidbody2D _rigidbody;
    private Controls _controls;

    private float _movementFloat;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _bounds = _collider.bounds;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (_controls is null)
        {
            _controls = new Controls();
            _controls.game.SetCallbacks(this);
        }
        _controls.game.Enable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        _movementFloat = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded() && context.performed)
        {
            _rigidbody.AddRelativeForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _jumpCount++;
        }
        else if (!IsGrounded() && _jumpCount > 0 && _jumpCount < maximumJumpCount && context.performed)
        {
            _rigidbody.AddRelativeForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _jumpCount++;
        }
    }

    private void FixedUpdate()
    {
        float force = _rigidbody.mass * acceleration;
        if (_rigidbody.velocity.x * _movementFloat <= maxMovementSpeed)
        {
            _rigidbody.AddForce(new Vector2(force * _movementFloat, 0), ForceMode2D.Force);
        }

        if (_movementFloat == 0)
        {
            if (_rigidbody.velocity.x > 0.1f)
            {
                _rigidbody.AddForce(new Vector2(-force / 2, 0));
            }
            if (_rigidbody.velocity.x < -0.1f)
            {
                _rigidbody.AddForce(new Vector2(force / 2, 0));
            }
            if (_rigidbody.velocity.x is > -0.1f and < 0.1f)
            {
                _rigidbody.velocity.Set(0, _rigidbody.velocity.y);
            }
        }

        if (_rigidbody.velocity.x >= maxMovementSpeed)
        {
            _rigidbody.velocity = new Vector2(maxMovementSpeed, _rigidbody.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (IsGrounded())
        {
            _jumpCount = 0;
        }
    }

    private bool IsGrounded()
    {
        Vector2 startPos = new(transform.position.x, transform.position.y - _bounds.extents.y + _collider.offset.y - raycastStartPosition);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(startPos, Vector2.down, raycastDistance);
        Debug.DrawRay(startPos, Vector3.down * raycastDistance, Color.red, Time.fixedDeltaTime);

        return raycastHit2D;
    }
}
