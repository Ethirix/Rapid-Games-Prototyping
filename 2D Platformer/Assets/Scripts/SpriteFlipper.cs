using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFlipper : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (playerController == null)
            return;

        _spriteRenderer.flipX = playerController.MovementFloat switch
        {
            > 0 => false,
            < 0 => true, 
            _ => _spriteRenderer.flipX
        };
    }
}
