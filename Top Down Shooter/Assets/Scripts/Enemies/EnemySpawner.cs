using UnityEngine;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        private EnemySpawnerTrigger _trigger;

        private void Start()
        {
            Transform trigger = transform.Find("Trigger");
            if (trigger == null)
                Reset();

            trigger = transform.Find("Trigger");

            _trigger = trigger.GetComponent<EnemySpawnerTrigger>();
        }

        private void Reset()
        {
            GameObject trigger = new("Trigger", typeof(BoxCollider2D), typeof(EnemySpawnerTrigger))
            {
                transform =
                {
                    parent = transform,
                    localPosition = new Vector3()
                }
            };
            trigger.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
}
