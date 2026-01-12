using Unity.VisualScripting;
using UnityEngine;

public class ParralaxRepeater : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        ParallaxLayer other = collision.otherCollider.GetComponent<ParallaxLayer>();
        if (other == null) return;

        if (!other.AutoRepeat) return;

        other.transform.position = new Vector3(
            respawnPoint.position.x,
            other.transform.position.y,
            other.transform.position.z
            );
    }

}
