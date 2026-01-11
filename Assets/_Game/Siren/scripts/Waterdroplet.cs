using UnityEngine;

public class WaterDroplet : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float lifetime = 8f;

    private void OnEnable() => Destroy(gameObject, lifetime);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
            Destroy(gameObject);
    }
}
