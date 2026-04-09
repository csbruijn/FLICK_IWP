using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    private float minSize;
    private float previousSize= 0f;
    bool isSizing = true;
    private float scrollspeed;
    private float currentPlatformSize = 0;

    [SerializeField] private float growSpeed = 2f;
    public void InitializePlatform(float scrollspeed, float minSize )
    {
        this.scrollspeed = scrollspeed; 
        this.minSize = minSize; 
    }

    public void StopSizing()
    {
        isSizing = false;
    }

    private void FixedUpdate()
    {
        if (isSizing)
        {
            float adjScrollspeed = scrollspeed * Time.fixedDeltaTime * growSpeed;
            currentPlatformSize += adjScrollspeed;

            Vector3 pos = transform.position;
            Vector3 scale = transform.localScale;

            scale.x = currentPlatformSize;
            transform.localScale = scale;

            //move platform to the left (currentScrollSpeed/2)
            pos.x += adjScrollspeed / 2;

            transform.position = pos;
        }
        else if (transform.localScale.x < minSize)
        {
            Destroy(gameObject);
        }
    }
}
