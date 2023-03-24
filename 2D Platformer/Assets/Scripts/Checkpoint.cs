using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Sprite sprite;

    public Transform Transform { get; private set; }
    public event EventHandler<Checkpoint> CheckpointHit;

    private GameManager _gameManager;
    private GameObject _beam;
    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        if (transform.parent.CompareTag("Checkpoint Manager"))
        {
            transform.parent = GameObject.FindGameObjectWithTag("Checkpoint Manager").transform;
        }

        Transform = gameObject.transform;
        tag = "Checkpoint";
    }

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        _gameManager.CheckpointManager.CheckpointChangedEvent += CheckpointChangedEvent;
        _collider = GetComponent<BoxCollider2D>();
        CreateBeam();
    }
    private void OnDisable()
    {
        _gameManager.CheckpointManager.CheckpointChangedEvent -= CheckpointChangedEvent;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CheckpointHit?.Invoke(this, this);
        }
    }
    private void CheckpointChangedEvent(object sender, Checkpoint checkpoint)
    {
        _spriteRenderer.color = checkpoint == this ? new Color(0, 255, 150) : Color.red;
    }

    private void CreateBeam()
    {
        _beam = new GameObject("Beam")
        {
            transform =
            {
                parent = transform,
                localScale = new Vector3(0.1f, _collider.size.y, 1),
                localPosition = new Vector3(0, _collider.offset.y, 1)
            }
        };
        _spriteRenderer = _beam.AddComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprite;

        _spriteRenderer.color = _gameManager.CheckpointManager.IsCheckpointActive(this) ? new Color(0, 255, 150) : Color.red;
    }
}
