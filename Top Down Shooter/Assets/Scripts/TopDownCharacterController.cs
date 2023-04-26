using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownCharacterController : MonoBehaviour
{
    [Header("Required GameObjects")] 
    [SerializeField]
    private new Camera camera;

    [Header("Movement parameters")]
    [SerializeField] 
    private float playerMaxSpeed = 100f;

    private Animator _animator;
    private Rigidbody2D _rb;

    private Vector2 _playerDirection;
    private float _playerSpeed = 1f;

    private readonly int _speed = Animator.StringToHash("Speed");
    private readonly int _horizontal = Animator.StringToHash("Horizontal");
    private readonly int _vertical = Animator.StringToHash("Vertical");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Vector3 mousePos = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;

        _animator.SetFloat(_horizontal, mousePos.x);
        _animator.SetFloat(_vertical, mousePos.y);

        _rb.velocity = _playerDirection * (_playerSpeed * playerMaxSpeed * Time.fixedDeltaTime);
    }

    public void OnPlayerInputShoot(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        Debug.Log($"Shoot! {Time.time}", gameObject);
    }

    public void OnPlayerInputMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _playerSpeed = 0f;
            _animator.SetFloat(_speed, 0);

            return;
        }

        if (!context.performed)
            return;

        Vector2 direction = context.ReadValue<Vector2>();
        _playerDirection = direction;

        _animator.SetFloat(_speed, _playerDirection.magnitude);

        _playerSpeed = 1f;
    }
}
