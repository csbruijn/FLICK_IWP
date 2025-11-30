using MidiJack;
using System.Collections;
using UnityEngine;

public class PianoKeyBooster : MonoBehaviour
{
    [SerializeField] MidiChannel myChannel;
    [SerializeField] int myNote;

    private GravityWell myGravityWell;
    [SerializeField] float BoostStrength = 8f; 

    [SerializeField] Color myColor = Color.white;

    private SpriteRenderer mSP;

    [SerializeField]private float ActiveDelay = .5f, extraTime =.3f;

    private float ActiveTime = 0f;

    private bool noteIsOn = false;
    private bool boosterIsActive;

    private void Awake()
    {
        mSP = GetComponent<SpriteRenderer>();
        mSP.color = myColor;
        myGravityWell = GetComponentInChildren<GravityWell>();
        myGravityWell.SetStrenght(BoostStrength);
        myGravityWell.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (noteIsOn)
        {
            if (ActiveTime == 0f) ActiveTime += ActiveDelay+extraTime; 
            ActiveTime += Time.deltaTime;
        }

        if (ActiveTime > 0) ActiveTime -= Time.deltaTime;

        if (ActiveTime <= 0)
        {
            DisableBooster();
            ActiveTime = 0; 
        }
    }

    void NoteOff(MidiChannel channel, int note)
    {
        if (channel != myChannel) return;

        if (note != myNote) return;
        
        if (myGravityWell.isActiveAndEnabled) DeacivateIndicator();
        //if (ActiveTime < 0.5f) ActiveTime = .5f; 
        noteIsOn = false;
        
    }

    void NoteOn(MidiChannel channel, int note, float velocity)
    {
        if (channel != myChannel) return;

        if (note != myNote) return;
        
        /*if (ActiveTime <= 0) */
        ActivateIndicator();

        Debug.Log($"{note} is pressed ");
        noteIsOn = true;
        StartCoroutine(DelayedActivation());
        
    }



    private IEnumerator DelayedActivation()
    {
        yield return new WaitForSeconds(ActiveDelay);
        ActivateBooster();
    }


    private void ActivateBooster()
    {
        DeacivateIndicator();
        myGravityWell.gameObject.SetActive(true);
    }

    private void DisableBooster()
    {
        myGravityWell.gameObject.SetActive(false);
    }
    private void ActivateIndicator()
    {
         mSP.color = Color.lightGreen;
    }

    private void DeacivateIndicator()
    {
        mSP.color = myColor;

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
