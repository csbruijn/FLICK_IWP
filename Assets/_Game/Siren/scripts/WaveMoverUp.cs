using UnityEngine;

public class WaveMoverUp : MonoBehaviour
{

    public float lifeTime = 5f;
    public float waveSpeed = 0.002f;

    void Start()
    {
        // delete them later
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {

        // move the wave to the right

        transform.position = new Vector3(transform.position.x, transform.position.y + waveSpeed, transform.position.z);
    }
}
