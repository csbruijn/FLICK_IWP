using Unity.VisualScripting;
using UnityEngine;
using FMODUnity;

public class ObjectiveNote : MonoBehaviour
{
    private float minimum = 0.1f;
    private float maximum = 0.5f;

    private float yPos;
    private float startYPos;
    private float bounceSpeed = 3;

    [SerializeField] GameEvent OnNotePickedUp;
    [SerializeField] private GameOutcome myOutcome;
    [SerializeField] private EventReference pickUpEvent;

    private void Start()
    {
        startYPos = transform.position.y; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return; 

        if (collision.GetComponent<PlayerStatus>().isDead ) return;

        OnNotePickedUp.Raise(this, myOutcome);
        RuntimeManager.PlayOneShot(pickUpEvent, transform.position);
        Destroy(this.gameObject);
    }

    void Update()
    {
        float sinValue = Mathf.Sin(Time.time * bounceSpeed);

        yPos = Mathf.Lerp(startYPos + maximum, startYPos - minimum, Mathf.Abs(sinValue));
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}
