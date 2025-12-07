using UnityEngine;

public class WaveMover : MonoBehaviour
{
 
    public float lifeTime = 5f;
    public float waveSpeed = 0.002f;

    void Start()
    {
        // Clean up after a while so they don’t pile up in the scene
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {

        // move the platform to the right

        transform.position = new Vector3(transform.position.x + waveSpeed, transform.position.y, transform.position.z);
    }
}
