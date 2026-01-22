using MidiJack;
using System.Collections;
using UnityEngine;

public class PianoKeyBooster : MonoBehaviour
{
    [Header("Midi setup")]
    [SerializeField] MidiChannel myChannel;
    [SerializeField] int myNote;

    [Header("Settings")]
    [SerializeField] float BoostStrength = 8f; 
    [SerializeField]private float ActiveDelay = .5f, extraTime =.3f;
    private float ActiveTime = 0f;
    private bool noteIsOn = false;

    [SerializeField] Color myColor = Color.white;
    private SpriteRenderer mSP;

    [Header("Fountain settings")]
    [SerializeField] private float indicatorHeight = 2f;
    [SerializeField] private float fullHeight = 8f;

    [Header("Refs")]
    [SerializeField]private GravityWell myGravityWell;
    [SerializeField] private  FountainController myFountainController;

    [SerializeField] private float maxResource = 1f;
    private float resource; 

    private void Awake()
    {
        resource = maxResource; 
        mSP = GetComponent<SpriteRenderer>();
        mSP.color = myColor;
        //myGravityWell = GetComponentInChildren<GravityWell>();
        myGravityWell.gameObject.SetActive(true);
        myGravityWell.SetStrenght(BoostStrength);
        myGravityWell.gameObject.SetActive(false);
    }

    private void Update()
    {

        if (noteIsOn && resource > 0)
        {
            resource -= Time.deltaTime;
            if (ActiveTime == 0f) ActiveTime += ActiveDelay+extraTime; 
            ActiveTime += Time.deltaTime;
        }
        else if (!noteIsOn && resource < maxResource)
        {
            resource += Time.deltaTime/2;
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
        
        //if (myGravityWell.isActiveAndEnabled) DeacivateIndicator();
        noteIsOn = false; 
    }

    void NoteOn(MidiChannel channel, int note, float velocity)
    {
        if (channel != myChannel) return;
        if (note != myNote) return;
        
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
        
        myFountainController.SetHeight(fullHeight);
        //DeacivateIndicator();
        myGravityWell.gameObject.SetActive(true);
    }

    private void DisableBooster()
    {
        myFountainController.GetComponent<ParticleSystem>().Stop();


        myGravityWell.gameObject.SetActive(false);
    }
    private void ActivateIndicator()
    {
        myFountainController.GetComponent<ParticleSystem>().Play();
        myFountainController.SetHeight(indicatorHeight);
         //mSP.color = Color.lightGreen;
    }

    //private void DeacivateIndicator()
    //{
    //    mSP.color = myColor;
    //}

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
