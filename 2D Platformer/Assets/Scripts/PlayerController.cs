using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D), typeof(Animator)), SelectionBase]
public class PlayerController : MonoBehaviour, Controls.IGameActions
{
    [Header("Base Settings")] 
    [SerializeField] private float maxMovementSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float slopeForce;

    [Header("Dash Settings")] 
    [SerializeField] private float dashForce;
    [SerializeField, Range(1, 10)] private int maximumDashCount;

    [Header("Jump Settings")] 
    [SerializeField, Range(1, 10)] private int maximumJumpCount;
    [SerializeField] private float jumpForce;

    [Header("Physics Settings")]
    [SerializeField] private float normalGravityModifier = 3f;
    [SerializeField] private float fallingGravityModifier = 10f;

    [Header("Floor Detection Settings")] 
    [SerializeField] private float raycastDistance;
    [SerializeField,
     Tooltip("From the base of the sprite, in World units, the starting position of the Raycast for the floor.")]
    private float raycastStartPosition;

    private int _jumpCount;
    private int _dashCount;

    private Collider2D _collider;
    private Bounds _bounds;
    private Rigidbody2D _rigidbody;
    private Controls _controls;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _doDash;
    private GameManager _gameManager;
    private bool _isGroundedThisFrame;

    public float MovementFloat { get; private set; }

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _bounds = _collider.bounds;
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = transform.Find("CharacterSprite").GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (_controls is null)
        {
            _controls = new Controls();
            _controls.game.SetCallbacks(this);
        }

        _controls.game.Enable();
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementFloat = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_isGroundedThisFrame && context.performed)
        {
            _rigidbody.AddRelativeForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _jumpCount++;
        }
        else if (!_isGroundedThisFrame && _jumpCount > 0 && _jumpCount < maximumJumpCount && context.performed)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);

            _rigidbody.AddRelativeForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _jumpCount++;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && _dashCount < maximumDashCount)
        {
            _doDash = true;
        }
    }

    private void FixedUpdate()
    {
        if (_gameManager.GameState != GameState.Playing)
            return;

        _isGroundedThisFrame = IsGrounded();
        RaycastHit2D wallCheck = _CheckWalls();

        if (_isGroundedThisFrame)
        {
            _jumpCount = 0;
            _dashCount = 0;
        }

        _animator.SetBool("Grounded", _isGroundedThisFrame);
        _animator.SetFloat("VelocityX", Mathf.Abs(_rigidbody.velocity.x));
        _animator.SetFloat("VelocityY", _rigidbody.velocity.y);

        float force = _rigidbody.mass * acceleration;
        Vector2 finalForce = Vector2.zero;

        if (Mathf.Abs(_rigidbody.velocity.x) <= maxMovementSpeed && MovementFloat != 0)
        { 
            finalForce += new Vector2(force * MovementFloat, 0);
        }
        else
        {
            switch (_rigidbody.velocity.x)
            {
                case < 0 when MovementFloat > 0:
                case > 0 when MovementFloat < 0:
                    finalForce += new Vector2(force * MovementFloat, 0);
                    break;
            }
        }

        switch (_rigidbody.velocity.x)
        {
            case > 0.1f when MovementFloat == 0:
                finalForce += new Vector2(-force / 2, 0);
                break;
            case < -0.1f when MovementFloat == 0:
                finalForce += new Vector2(force / 2, 0);
                break;
            case > -0.1f and < 0.1f when MovementFloat == 0:
                _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
                break;
        }

        if (Mathf.Abs(_rigidbody.velocity.x) >= maxMovementSpeed)
        {
            _rigidbody.AddForce(new Vector2(-_rigidbody.velocity.x, 0));
        }

        _rigidbody.AddForce(
            _rigidbody.velocity.y < 0
                ? new Vector2(0, Physics.gravity.y * fallingGravityModifier)
                : new Vector2(0, Physics.gravity.y * normalGravityModifier), ForceMode2D.Impulse);

        IsGrounded(out RaycastHit2D raycastHit2D);
        if (raycastHit2D && MovementFloat != 0 && raycastHit2D.normal.normalized != Vector2.up)
        {
            finalForce += new Vector2(-raycastHit2D.normal.x, raycastHit2D.normal.y) * slopeForce;
            Debug.DrawLine(transform.position - raycastHit2D.collider.gameObject.transform.position,
                transform.position - new Vector3(-raycastHit2D.normal.x, raycastHit2D.normal.y, 0), Color.green,
                Time.fixedDeltaTime);
        }

        if (!wallCheck.collider || wallCheck.collider.isTrigger)
        {
            _rigidbody.AddForce(finalForce, ForceMode2D.Force);
        }

        if (_doDash && Mathf.Abs(_rigidbody.velocity.x) > 0.1f && _dashCount < maximumDashCount)
        {
            _doDash = false;
            _dashCount++;
            _animator.SetTrigger("Dash");
            _rigidbody.AddForce(new Vector2(_rigidbody.velocity.normalized.x * dashForce, 0f), ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return _CheckGrounded();
    }

    private bool IsGrounded(out RaycastHit2D raycastHit2D)
    {
        raycastHit2D = _CheckGrounded();
        return raycastHit2D;
    }

    private RaycastHit2D _CheckGrounded()
    {
        Vector2 startPos = new(transform.position.x,
            transform.position.y - _bounds.extents.y + _collider.offset.y - raycastStartPosition);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(startPos, Vector2.down, raycastDistance);

        Debug.DrawRay(startPos, 10f * raycastDistance * Vector3.down, Color.red, Time.fixedDeltaTime);

        return raycastHit2D;
    }

    private RaycastHit2D _CheckWalls()
    {
        int rayDirection = _spriteRenderer.flipX switch
        {
            true => -1,
            false => 1
        };

        Vector2 startPos = new(transform.position.x + rayDirection * _bounds.extents.x + rayDirection * _collider.offset.x + raycastStartPosition, transform.position.y + _collider.offset.y);
        Vector2 direction = _spriteRenderer.flipX switch
        {
            true => Vector2.left,
            false => Vector2.right
        };

        RaycastHit2D raycastHit2D = Physics2D.Raycast(startPos, direction, raycastDistance);
        Debug.DrawRay(startPos, 10f * raycastDistance * direction, Color.red, Time.fixedDeltaTime);

        return raycastHit2D;
    }
}