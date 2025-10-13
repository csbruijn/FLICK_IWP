using MidiJack;
using UnityEngine;
using UnityEngine.WSA;

public class PianoKeyBooster : MonoBehaviour
{

    [SerializeField] MidiChannel myChannel;
    [SerializeField] int myNote;


    private GravityWell myGravityWell;
    [SerializeField] float BoostStrength = 8f; 

    [SerializeField] Color myColor = Color.white;

    private SpriteRenderer mSP; 

    private void Awake()
    {
        mSP = GetComponent<SpriteRenderer>();
        mSP.color = myColor;
        myGravityWell = GetComponentInChildren<GravityWell>();
        myGravityWell.SetStrenght(BoostStrength);
        myGravityWell.gameObject.SetActive(false);
    }

    void ActivateBooster()
    {
        mSP.color = Color.lightGreen; 

        myGravityWell.gameObject.SetActive( true );
    }
    private void DisableBooster()
    {
        mSP.color = myColor;
        myGravityWell.gameObject.SetActive(false);
    }

    void NoteOff(MidiChannel channel, int note)
    {
        DisableBooster();
    }

    void NoteOn(MidiChannel channel, int note, float velocity)
    {
        if (note == myNote)
        {
            ActivateBooster();
        }
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
