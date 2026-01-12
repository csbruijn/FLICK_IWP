using FMODUnity;
using MidiJack;
using UnityEngine;

public class NotesSpawner : MonoBehaviour
{
    [Header("midi setup")]
    [SerializeField] private int maxMidi = 0, minMidi = 127;
    private int[] myMidiNotes;

    [Header("Spawn setup")]
    [SerializeField] private float yMax;
    [SerializeField] private float yMin;
    [SerializeField] private float xOffset =12f;

    private float increments;

    [Header("Refs")]
    [SerializeField] private ObjectiveNote prefab;
    [SerializeField] private Transform origin, platformsParent;

    private double lastNoteTime = -1; 

    private void Start()
    {
        int range = maxMidi - minMidi;
        increments = (yMax - yMin) / range;
        Debug.Log(increments);

        myMidiNotes = new int[range];

        for (int i = 0; i < range; i++)
        {
            myMidiNotes[i] = minMidi + i;
        }
    }

    public void OnSaxNotePlayed(Component Sender, System.Object Data)
    {
        MidiNoteEvent mnd = (MidiNoteEvent)Data;

        if (mnd.note < minMidi || mnd.note > maxMidi)
            return;

        float height = (mnd.note - minMidi) * increments;

        float str; 

        if (lastNoteTime < 0)
        {
            lastNoteTime = 0;
            str = (float)mnd.time;
        }
        else
        {
            str = (float)mnd.time - (float)lastNoteTime;
        }

        lastNoteTime = mnd.time;

        CreateNote(height, str);
    }
    private void CreateNote(float height, float strength)
    {
        //Debug.Log($"create a Note: {origin.position}");

        Vector3 spawnPos = new Vector3(
            origin.position.x + xOffset,
            origin.position.y + height - ((yMax - yMin) / 2),
            origin.position.z);

        ObjectiveNote n =  Instantiate(prefab, spawnPos, Quaternion.identity);
        n.transform.SetParent(platformsParent);
        n.SetStrength(strength); 
    }
}
