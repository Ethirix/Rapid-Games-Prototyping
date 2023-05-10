using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

[RequireComponent(typeof(WeaponController), typeof(Rigidbody2D), typeof(Animator))]
public class TopDownCharacterController : MonoBehaviour
{
    [Header("Required GameObjects")] 
    [SerializeField]
    private new Camera camera;
    [SerializeField]
    private GameObject flashlight;

    [Header("Movement parameters")]
    [SerializeField] 
    private float playerMaxSpeed = 100f;

    private Animator _animator;
    private Rigidbody2D _rb;
    private WeaponController _weaponController;

    private int _currentWeapon;

    private Vector2 _playerDirection;
    private Vector3 _mousePos;
    private float _playerSpeed = 1f;

    private readonly int _speed = Animator.StringToHash("Speed");
    private readonly int _horizontal = Animator.StringToHash("Horizontal");
    private readonly int _vertical = Animator.StringToHash("Vertical");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _weaponController = GetComponent<WeaponController>();
    }

    private void FixedUpdate()
    {
        _mousePos = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        
        Quaternion rotation = flashlight.transform.rotation;
        rotation = Quaternion.Euler(rotation.x, rotation.y, -Quaternion.LookRotation(_mousePos, Vector3.forward).eulerAngles.z);
        flashlight.transform.rotation = rotation;

        _animator.SetFloat(_horizontal, _mousePos.x);
        _animator.SetFloat(_vertical, _mousePos.y);

        _rb.velocity = _playerDirection * (_playerSpeed * playerMaxSpeed * Time.fixedDeltaTime);
    }

    public void OnPlayerInputReload(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        _weaponController.Reload();

        Debug.Log($"Reload - {Time.time}", gameObject);
    }

    public void OnPlayerInputShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _weaponController.Shoot(flashlight.transform, flashlight.transform);
        } 
        else if (context.canceled)
        {
            _weaponController.StopShooting();
        }
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

    public void OnPlayerInputSwitchWeapons(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        int val = (int)context.ReadValue<float>();

        switch (val)
        {
            case > 0:
            {
                if (_currentWeapon < _weaponController.GetWeaponsSize() - 1)
                    _currentWeapon++;
                break;
            }
            case < 0:
            {
                if (_currentWeapon > 0)
                    _currentWeapon--;
                break;
            }
        }

        _weaponController.EquipWeapon(_currentWeapon);
    }
}
