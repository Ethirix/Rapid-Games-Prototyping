using UnityEngine;

public class ExtendMap : MonoBehaviour
{
    [SerializeField] private GameObject mapPrefab;
    [SerializeField] private float beyondViewRenderDistance;

    private Camera _mainCamera;
    private Collider2D _collider;
    private Resolution _resolution;
    private Bounds _bounds;

    private bool ranSpawn;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _resolution.width = Screen.width;
        _resolution.height = Screen.height;
        _collider = GetComponent<Collider2D>();

        _bounds = _collider.bounds;
    }

    private void FixedUpdate()
    {
        Vector3 topPos = _mainCamera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + _bounds.extents.y - beyondViewRenderDistance), Camera.MonoOrStereoscopicEye.Mono);
        Vector3 bottomPos = _mainCamera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + _bounds.extents.y + beyondViewRenderDistance), Camera.MonoOrStereoscopicEye.Mono);

        if (_resolution.height - topPos.y > 0 && !ranSpawn)
        {
            ranSpawn = true;
            GameObject map = Instantiate(mapPrefab, GameObject.Find("Background").transform);
            map.transform.position = new Vector3(transform.position.x, transform.position.y + _bounds.extents.y * 2);
        }

        if (_resolution.height - bottomPos.y > _resolution.height)
        {
            Destroy(gameObject);
        }
    }
}
