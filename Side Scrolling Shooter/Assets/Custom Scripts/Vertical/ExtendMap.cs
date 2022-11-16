using UnityEngine;

public class ExtendMap : MonoBehaviour
{
    [SerializeField] private GameObject mapPrefab;

    private Camera camera;
    private Resolution resolution;
    private Collider2D collider;
    private Bounds bounds;

    private bool ranSpawn;

    private void Awake()
    {
        camera = Camera.main;
        resolution = Screen.currentResolution;
        collider = GetComponent<Collider2D>();

        bounds = collider.bounds;
    }

    private void FixedUpdate()
    {

        Vector3 pos = camera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + bounds.extents.y + 1), Camera.MonoOrStereoscopicEye.Mono);
        

        if (pos.y - resolution.height < 10 && !ranSpawn)
        {
            ranSpawn = true;
            GameObject map = Instantiate(mapPrefab);
            map.transform.parent = GameObject.Find("Background").transform;
            map.transform.position = new Vector3(transform.position.x, transform.position.y + bounds.extents.y);
        }

        //if (pos.y > resolution.height && !ranSpawn)
        //{
        //    ranSpawn = true;
        //    GameObject map = Instantiate(mapPrefab);
        //    map.transform.parent = GameObject.Find("Background").transform;
        //    map.transform.position = new Vector3(transform.position.x, transform.position.y + bounds.extents.y);
        //}
    }
}
