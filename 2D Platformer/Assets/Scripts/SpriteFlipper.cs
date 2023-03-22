using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFlipper : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (rigidbody == null)
            return;

        _spriteRenderer.flipX = rigidbody.velocity.x switch
        {
            > 0.15f => false,
            < -0.15f => true,
            _ => _spriteRenderer.flipX
        };
    }
}
