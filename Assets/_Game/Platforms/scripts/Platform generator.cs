using MidiJack;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Burst.Intrinsics.Arm;

public class Platformgenerator : MonoBehaviour
{
    [Header("midi setup")]
    [SerializeField] MidiChannel myChannel;
    [SerializeField] private int maxMidi =0, minMidi = 127;
    private int[] myMidiNotes;

    [Header("Spawn setup")]
    [SerializeField] private float yMax;
    [SerializeField] private float yMin;
    [SerializeField] private float minPlatformSize = 0.1f;
    private bool creatingPlatform = false;
    private float currentPlatformSize = 0f, increments;
    private float scrollspeed;
    [SerializeField] private float xOffset = 10f;

    [Header("Refs")]
    [SerializeField] private GameObject platform;
    [SerializeField] private Transform origin, platformsParent;
    private Dictionary<int ,GameObject> currentPlatforms; 

    private void Start()
    {
        scrollspeed = Gamemanager.instance.currentScrollSpeed;
        currentPlatforms = new();
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


    private void CreatePlatform(int note)
    {
        float height = (note - minMidi) * increments;
        creatingPlatform = true;
        currentPlatformSize = 0f;
        Debug.Log($"create a platform: {origin.position}");

        Vector3 spawnPos = new Vector3(
            origin.position.x + xOffset,
            origin.position.y + height - ((yMax - yMin)/2),
            origin.position.z);

        GameObject currentPlatform =  Instantiate(platform, spawnPos, Quaternion.identity);
        currentPlatform.transform.SetParent(platformsParent);

        PlatformBehaviour pb = currentPlatform.GetComponent<PlatformBehaviour>();
        pb.InitializePlatform(scrollspeed, minPlatformSize); 
        currentPlatforms.Add(note ,currentPlatform);
    }



    void NoteOff(MidiChannel channel, int note)
    {
        if (channel != myChannel) return;
        if (note > maxMidi || note < minMidi) return;

        GameObject pb; 
        currentPlatforms.TryGetValue(note, out pb);

        pb.GetComponent<PlatformBehaviour>().StopSizing(); 
        currentPlatforms.Remove(note );

        //creatingPlatform = false;
    }

    

    void NoteOn(MidiChannel channel, int note, float velocity)
    {
        if (channel != myChannel) return;
        if (note > maxMidi || note < minMidi) return;
      
        CreatePlatform(note) ;
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
