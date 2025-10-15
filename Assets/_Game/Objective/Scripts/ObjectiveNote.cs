using Unity.VisualScripting;
using UnityEngine;

public class ObjectiveNote : MonoBehaviour
{
    private float minimum = 0.1f;
    private float maximum = 0.5f;

    private float yPos;
    private float startYPos;
    private float bounceSpeed = 3;

    private void Start()
    {
        startYPos = transform.position.y; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         
        Destroy(this.gameObject);
    }

    void Update()
    {
        float sinValue = Mathf.Sin(Time.time * bounceSpeed);

        yPos = Mathf.Lerp(startYPos + maximum, startYPos - minimum, Mathf.Abs(sinValue));
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);

    }
}
