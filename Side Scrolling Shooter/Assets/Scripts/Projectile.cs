using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(PolygonCollider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private Sprite sprite;
    public int Damage { get; private set; }


    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Damage = damage;
    }
}
