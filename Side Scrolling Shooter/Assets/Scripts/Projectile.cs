using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private int damage;
    [SerializeField] private float speed;

    [Header("Object Settings")]
    [SerializeField] private float time;
    [SerializeField] private Sprite sprite;

    public int Damage { get; private set; }

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprite;
        gameObject.AddComponent<PolygonCollider2D>();
        gameObject.tag = "Projectile";
        Damage = damage;
        StartCoroutine(DeletionTimer());
    }

    private IEnumerator DeletionTimer()
    {
        float timer = 0;
        while (true)
        {
            timer += Time.fixedDeltaTime;

            if (timer > time)
            {
                Destroy(gameObject);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(0, speed * Time.fixedDeltaTime), Space.Self);
    }
}