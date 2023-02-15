using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] 
    private float movementSpeed;
    

    [Header("Jump Settings")]
    [SerializeField] 
    private int maximumJumpCount;
    [SerializeField]
    private float raycastDistance;
    [SerializeField, Tooltip("From the base of the sprite, in World units, the starting position of the Raycast for the floor.")] 
    private float raycastStartPosition;

    private int _jumpCount = 0;

    private Collider2D _collider;
    private Bounds _bounds;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _bounds = _collider.bounds;
    }

    private void FixedUpdate()
    {
        print(IsGrounded());
    }

    private bool IsGrounded()
    {
        Vector2 startPos = new(_bounds.center.x, _bounds.center.y - _bounds.extents.y - raycastStartPosition - _collider.offset.y);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(startPos, Vector2.down);
        Debug.DrawRay(startPos, Vector3.down, Color.red, Time.fixedDeltaTime);

        return raycastHit2D;
    }
}
