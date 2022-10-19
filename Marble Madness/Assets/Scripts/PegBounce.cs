using UnityEngine;

public class PegBounce : MonoBehaviour
{
    [SerializeField, Range(500f, 10000f)] private float bounceScale;
    [SerializeField] private int scoreModifier;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerController playerController;
            collision.collider.TryGetComponent<PlayerController>(out playerController);

            if (playerController is not null)
            {
                Vector3 playerPos = playerController.transform.position;
                Vector3 pegPos = transform.position;

                Vector3 towardsPlayer = Vector3.Normalize(playerPos - pegPos);
                playerController.RB.AddForce(towardsPlayer * bounceScale, ForceMode.Force);

                playerController.addScore(scoreModifier);
            }
        }
    }

}
