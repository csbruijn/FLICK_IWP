using Unity.VisualScripting;
using UnityEngine;
using FMODUnity;
using Unity.Mathematics;

public class ObjectiveNote : MonoBehaviour
{

    [SerializeField] float minStr = 0.1f;
    [SerializeField] float maxStr = 3f;

    private float strength;

    [SerializeField] private float minimum = 0.1f;
    [SerializeField] private float maximum = 0.5f;

    private float yPos;
    private float startYPos;
    [SerializeField] private float bounceSpeed = 3;

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

        OnNotePickedUp.Raise(this, strength);
        RuntimeManager.PlayOneShot(pickUpEvent, transform.position);
        Destroy(this.gameObject);
    }

    void Update()
    {
        float sinValue = Mathf.Sin(Time.time * bounceSpeed);

        yPos = Mathf.Lerp(startYPos + maximum, startYPos - minimum, Mathf.Abs(sinValue));
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }

    

    public void SetStrength(float str)
    {
         

       if (str < minStr) Destroy(this.gameObject);

       if (str > maxStr) str = maxStr; 

       strength = str;

       transform.localScale = new Vector3(strength, strength, strength);  
    }
}
