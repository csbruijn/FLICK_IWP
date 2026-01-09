using UnityEngine;

public class MovingFrame : MonoBehaviour
{
    private float scrollSpeed;

    private void Start()
    {
        scrollSpeed = Gamemanager.instance.currentScrollSpeed;
    }

    private void FixedUpdate()
    {
        if (!Gamemanager.instance.GameStarted) return;

        transform.position = new Vector3(
            transform.position.x + (scrollSpeed *Time.fixedDeltaTime), 
            transform.position.y, 
            transform.position.z);

    }
}
