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
    private Dictionary<int ,GameObject> currentPlatforms =new();


    [SerializeField] private float maxResource = 1f;
    private float resource;

    private Dictionary<int, float> myMidiNotes;
    private void Update()
    {
        for (int midi = minMidi; midi <= maxMidi; midi++)
        {
            float r;
                myMidiNotes.TryGetValue(midi, out r);
            
            if (r > 0f && currentPlatforms.ContainsKey(midi))
            {
                r -= Time.deltaTime;

                if (r <= 0f)
                {
                    r = 0f;
                    NoteOff(myChannel, midi);
                }
            }
            else if (r < maxResource) 
            {
                r += Time.deltaTime * 0.5f;
            }

            myMidiNotes[midi] = Mathf.Clamp(r, 0f, maxResource);
        }
    }


    private void Start()
    {
        scrollspeed = Gamemanager.instance.currentScrollSpeed;
        currentPlatforms = new();
        // array of all the midi notes we want to play. 
        int range = maxMidi - minMidi;
        increments = (yMax - yMin)/range;
        Debug.Log(increments);
        resource = maxResource; 

        myMidiNotes = new Dictionary<int, float>();

        for (int i = 0; i < range; i++)
        {
            myMidiNotes.Add(i+minMidi, resource); 
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
        if (note < minMidi || note > maxMidi) return;

        if (!currentPlatforms.TryGetValue(note, out GameObject platform))
            return;

        platform.GetComponent<PlatformBehaviour>()?.StopSizing();
        currentPlatforms.Remove(note);
    }




    void NoteOn(MidiChannel channel, int note, float velocity)
    {
        if (!myMidiNotes.ContainsKey(note)) return;

        if (channel != myChannel) return;
        if (note > maxMidi || note < minMidi) return;
        float r;
        myMidiNotes.TryGetValue(note, out r);
        if (r <= 0.1) return; 

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
