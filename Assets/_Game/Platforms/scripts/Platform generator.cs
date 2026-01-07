using MidiJack;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Burst.Intrinsics.Arm;

public class Platformgenerator : MonoBehaviour
{

    [SerializeField] private GameObject platform;
    [SerializeField] private Transform origin, platformsParent;

    [SerializeField] MidiChannel myChannel;

    [SerializeField] private int maxMidi =0, minMidi = 127;
    private int[] myMidiNotes;

    [SerializeField] private float yMax, yMin;

    private float scrollspeed;

    private bool mousedown = false, creatingPlatform = false;

    private float currentPlatformSize = 0f, increments;

    private GameObject currentPlatform; 




    private void Start()
    {
        scrollspeed = Gamemanager.instance.currentScrollSpeed;

        // array of all the midi notes we want to play. 
        int range = maxMidi - minMidi;
        increments = (yMax - yMin)/range;
        Debug.Log(increments);

        myMidiNotes = new int[range];

        for (int i = 0; i < range; i++)
        {
            myMidiNotes[i] = minMidi + i;
            //Debug.Log(myMidiNotes[i]);
        }
    }

    
    private void FixedUpdate()
    {
        if (creatingPlatform)
        { ScalePlatform(); }
    }

    private void CreatePlatform(float height)
    {
        creatingPlatform = true;
        currentPlatformSize = 0f;
        Debug.Log($"create a platform: {origin.position}");

        Vector3 spawnPos = new Vector3(
            origin.position.x + 12,
            origin.position.y + height - ((yMax - yMin)/2),
            origin.position.z);

        currentPlatform =  Instantiate(platform, spawnPos, Quaternion.identity);
        currentPlatform.transform.SetParent(platformsParent);
        
        ScalePlatform();         

    }

    private void ScalePlatform()
    {
        currentPlatformSize += scrollspeed;
        //Debug.Log($"size: {currentPlatformSize}");

        Vector3 pos = currentPlatform.transform.position;

        // increasing the x-scale with currentScrollSpeed. 
        Vector3 scale = currentPlatform.transform.localScale;
        scale.x = currentPlatformSize;       
        currentPlatform.transform.localScale = scale;

        //move platform to the left (currentScrollSpeed/2)
        pos.x += scrollspeed / 2;

        currentPlatform.transform.position = pos;
    }


    void NoteOff(MidiChannel channel, int note)
    {
        if (channel != myChannel) return;

        if (note > maxMidi || note < minMidi) return;

        creatingPlatform = false;
    }

    void NoteOn(MidiChannel channel, int note, float velocity)
    {
        if (channel != myChannel) return;
        
        if (note > maxMidi || note < minMidi) return;
      
        CreatePlatform((note - minMidi) * increments) ;
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
