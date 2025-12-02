using UnityEngine;

public class MovingFrame : MonoBehaviour
{
    [SerializeField] private float sideScrollSpeed = 0.002f;

    private void FixedUpdate()
    {

        // move the platform to the left 

        transform.position = new Vector3(transform.position.x + sideScrollSpeed, transform.position.y, transform.position.z);

    }
}
