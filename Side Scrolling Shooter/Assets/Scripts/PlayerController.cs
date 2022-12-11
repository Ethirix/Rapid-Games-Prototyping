
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : Entity
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float movementForce;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float cooldown = 0.5f;

    private float _horizontalInput;
    private float _verticalInput;

    private VerticalPushCamera _camera;

    protected override void Start()
    {
        base.Start();
        _camera = Camera.main.GetComponent<VerticalPushCamera>();
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + (_camera.speed * Time.deltaTime));

        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
    }

    protected override void FixedUpdate()
    {
        MovementLogic();
        if (Input.GetButton("Fire1"))
        {
            FiringLogic();
        }
    }

    protected override void MovementLogic()
    {
        rigidbody2D.AddForce(new Vector2(movementForce * _horizontalInput, movementForce * _verticalInput));

        if (Mathf.Abs(rigidbody2D.velocity.sqrMagnitude) > maxSpeed * maxSpeed)
        {
            rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, maxSpeed);
        }
    }

    protected override void FiringLogic()
    {
        List<FiringPoint> activeFiringPoints = firingPoints.Where(fP => !fP.OnCooldown).ToList();

        foreach (FiringPoint firingPoint in activeFiringPoints)
        {
            StartCoroutine(firingPoint.RunCooldown());
            Instantiate(projectilePrefab, firingPoint.Transform.position, firingPoint.Transform.rotation).GetComponent<Projectile>();
        }
    }
}
