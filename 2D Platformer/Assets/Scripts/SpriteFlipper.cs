using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFlipper : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private SpriteRenderer _spriteRenderer;
    private GameManager _gameManager;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (_gameManager.GameState != GameState.Playing || playerController == null)
            return;

        _spriteRenderer.flipX = playerController.MovementFloat switch
        {
            > 0 => false,
            < 0 => true, 
            _ => _spriteRenderer.flipX
        };
    }
}
