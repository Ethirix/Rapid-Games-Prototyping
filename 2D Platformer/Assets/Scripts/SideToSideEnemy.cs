using System;
using UnityEngine;

public class SideToSideEnemy : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private GameObject _rightPoint;
    private GameObject _leftPoint;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _rightPoint = transform.parent.Find("Right").gameObject;
        _leftPoint = transform.parent.Find("Left").gameObject;

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            gameManager.PlayerRespawnManager.RespawnPlayer();
        }
    }

    private void Update()
    {
        if (_direction == Vector2.right && transform.position.x > _rightPoint.transform.position.x)
            _direction = Vector2.left;
        else if (_direction == Vector2.left && transform.position.x < _leftPoint.transform.position.x)
            _direction = Vector2.right;

        if (_direction == Vector2.right)
            _spriteRenderer.flipX = false;
        else if (_direction == Vector2.left)
            _spriteRenderer.flipX = true;
            

        transform.Translate(_direction * (Time.deltaTime * 2f));
    }
}
