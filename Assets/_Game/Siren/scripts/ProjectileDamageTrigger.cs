using UnityEngine;

public class ProjectileDamageTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private bool destroyOnHit = true;

    // Optional: prevent multi-hits if the trigger overlaps multiple colliders
    private bool hasHit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        // Only react to Player layer
        if (((1 << other.gameObject.layer) & playerLayer) == 0) return;

        // Your health script is PlayerStatus
        PlayerStatus status = other.GetComponentInParent<PlayerStatus>();
        if (status == null) return;

        // Don’t damage dead players
        if (status.isDead) return;

        // This matches your PlayerStatus API:
        status.DamagePlayer();

        hasHit = true;

        if (destroyOnHit)
        {
            Destroy(transform.root.gameObject); // destroys the whole droplet
        }
    }
}
