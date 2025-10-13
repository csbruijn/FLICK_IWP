using UnityEngine;

public class ScrollerPlatform : MonoBehaviour
{
    public GameEvent OnPlatformEnd;

    public float xMinBorder;
    [SerializeField] float platformSpeed = .05f;

    private void FixedUpdate()
    {
        // move the platform to the left 

        transform.position = new Vector3(transform.position.x - platformSpeed, transform.position.y, transform.position.z);
        
        // if far enough, remove platform 

        if (transform.position.x < xMinBorder) RemovePlatform();
    }

    void RemovePlatform()
    {
        OnPlatformEnd.Raise(this, transform.position.y);
        Destroy(gameObject);
    }

}
