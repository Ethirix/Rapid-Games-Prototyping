using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BasicEnemy : Entity
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float sideToSideWidth = 5;
    [SerializeField] private float sidewaysMovementForce = 15;
    [SerializeField] private float forwardMovementForce = 10;
    [SerializeField] private float detectionRange = 10;
    
    private int _direction = 1;

    protected override void MovementLogic()
    {
        if (transform.position.x < -sideToSideWidth)
        {
            _direction = 1;
        }

        if (transform.position.x > sideToSideWidth)
        {
            _direction = -1;
        }

        rigidbody2D.AddRelativeForce(new Vector2(sidewaysMovementForce * _direction, -forwardMovementForce));
    }

    protected override void FiringLogic()
    {
        List<FiringPoint> activeFiringPoints = firingPoints.Where(fP => !fP.OnCooldown).ToList();

        foreach (FiringPoint firingPoint in activeFiringPoints)
        {
            RaycastHit2D hit = Physics2D.Raycast(firingPoint.Transform.position,  Vector2.down, detectionRange, LayerMask.GetMask("Player"));
#if UNITY_EDITOR
            Debug.DrawRay(firingPoint.Transform.position,  Vector3.down * detectionRange, Color.red);
#endif
            if (hit.collider == null || !hit.collider.gameObject.CompareTag("Player")) continue;

            StartCoroutine(firingPoint.RunCooldown());
            Instantiate(projectilePrefab, firingPoint.Transform.position, firingPoint.Transform.rotation).GetComponent<Projectile>();
        }
    }
}