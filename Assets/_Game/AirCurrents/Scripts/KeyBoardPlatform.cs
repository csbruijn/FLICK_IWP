using MidiJack;
using UnityEngine;

public class KeyBoardPlatform : MonoBehaviour
{
    [SerializeField] MidiChannel myChannel;
    [SerializeField] int myNote;

    Material myMaterial;
    Rigidbody myRigidbody;

    [SerializeField] float maxHeight, restPos;
    bool jumpUp = false;
    bool lower = false;

    [SerializeField] float jumpSpeed =5f, lowerSpeed =1f , launchSpeed= 10f;

    public GameEvent keyReachedApex; 

    private void Awake()
    {            
        myRigidbody = GetComponent<Rigidbody>();
        myMaterial = GetComponentInChildren<Renderer>().material;

        restPos = transform.position.z;
        myMaterial.color = Color.green;

    }

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;
        if (jumpUp)
        {
            pos.y += jumpSpeed * Time.fixedDeltaTime;
            if (pos.y >= maxHeight)
            {
                pos.y = maxHeight;
                jumpUp = false;
                lower = true;
                keyReachedApex.Raise(this, launchSpeed);
            }
        }
        if (lower)
        {
            pos.y -= lowerSpeed * Time.fixedDeltaTime;

            if (pos.y <= restPos)
            {
                pos.y = restPos;
                lower = false;
                myMaterial.color = Color.green;
            }
        }
        myRigidbody.MovePosition(pos);

    }


    void ActivatePlatform()
    {
        myMaterial.color = Color.red;
        jumpUp = true;
    }

    void NoteOn(MidiChannel channel, int note, float velocity)
    {
        if (note == myNote) 
        { 
            ActivatePlatform();
        } 
    }

    void NoteOff(MidiChannel channel, int note)
    {
    }

    void OnEnable()
    {
        MidiMaster.noteOnDelegate += NoteOn;
        MidiMaster.noteOffDelegate += NoteOff;
    }

    void OnDisable()
    {
        MidiMaster.noteOnDelegate -= NoteOn;
        MidiMaster.noteOffDelegate -= NoteOff;
    }
}
