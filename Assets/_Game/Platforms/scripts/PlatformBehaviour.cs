using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    public float minSize;
    private float previousSize= 0f;
    bool isSizing = true; 

    private void FixedUpdate()
    {
        if (!isSizing) return;

        if (transform.localScale.x > previousSize)
        {
            previousSize = transform.localScale.x;
            return;
        }

        if (transform.localScale.x <= minSize) Destroy(gameObject);
        else isSizing = false; 
    }
}
