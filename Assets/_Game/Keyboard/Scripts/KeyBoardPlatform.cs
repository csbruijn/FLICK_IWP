using MidiJack;
using UnityEngine;

public class KeyBoardPlatform : MonoBehaviour
{
    [SerializeField] MidiChannel myChannel;
    [SerializeField] int myNote;

    Material myMaterial;

    [SerializeField] float maxHeight, restPos;
    bool jumpUp = false;
    bool lower = false;

    [SerializeField] float jumpSpeed, lowerSpeed; 

    private void Awake()
    {
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

        transform.position = pos;
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
        Debug.Log("NoteOff: " + channel + "," + note);
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
