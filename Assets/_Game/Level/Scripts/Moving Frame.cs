using UnityEngine;

public class MovingFrame : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;

    private void Start()
    {
        scrollSpeed = Gamemanager.instance.scrollspeed;
    }

    private void FixedUpdate()
    {
        // move the platform to the left 
        transform.position = new Vector3(
            transform.position.x + scrollSpeed, 
            transform.position.y, 
            transform.position.z);

    }
}
