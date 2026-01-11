using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{
    [Header("MIDI")]
    //[SerializeField] private string midiFileName = "basic_pitch_transcription(1).mid";

    private MidiFile midiFile;
    private TempoMap tempoMap;
    private List<MidiNoteEvent> events;

    private float songStartTime;
    private int index;
    private bool songPlaying;

    [SerializeField] private string MidifilePath; 

    [Header("Events")]
    [SerializeField] private GameEvent OnSaxNotePlayed;
    [SerializeField] private GameEvent OnSirenWaveAttack;
    [SerializeField] private GameEvent OnSirenSplashAttack;



    private void Awake()
    {
        LoadMidi();
        InitTrack();
    }

    private void Start()
    {
        //StartSong();
    }

    private void LoadMidi()
    {
        midiFile = MidiFile.Read(MidifilePath);


        //// Place MIDI in StreamingAssets
        //string path = System.IO.Path.Combine(
        //    Application.streamingAssetsPath,
        //    midiFileName
        //);

        //midiFile = MidiFile.Read(path);
        tempoMap = midiFile.GetTempoMap();
    }

    private void InitTrack()
    {
        events = new List<MidiNoteEvent>();

        foreach (var note in midiFile.GetNotes())
        {
            double time =
                note.TimeAs<MetricTimeSpan>(tempoMap).TotalSeconds;

            events.Add(new MidiNoteEvent
            {
                time = time,
                note = note.NoteNumber,
                velocity = note.Velocity
            });
        }

        events.Sort((a, b) => a.time.CompareTo(b.time));

        //  NORMALIZE TIME
        double firstNoteTime = events[0].time;
        for (int i = 0; i < events.Count; i++)
        {
            events[i] = new MidiNoteEvent
            {
                time = events[i].time - firstNoteTime,
                note = events[i].note,
                velocity = events[i].velocity
            };
        }

        index = 0;

        Debug.Log($"First normalized note at: {events[0].time}");
    }


    public void StartSong(Component sender, System.Object Data)
    {
        if (songPlaying)
            return;

        Debug.Log("Start Track");
        songStartTime = Time.timeSinceLevelLoad;
        songPlaying = true;
    }


    private void Update()
    {
        if (!songPlaying || index >= events.Count)
            return;

        float songTime = Time.timeSinceLevelLoad - songStartTime;

        while (index < events.Count && events[index].time <= songTime)
        {
            OnMidiNote(events[index]);
            index++;
        }
    }


    private void OnMidiNote(MidiNoteEvent e)
    {
        Debug.Log($"NOTE | t={e.time:F3} | note={e.note} | vel={e.velocity}");

        // Hook gameplay here
        // SpawnEnemy(e.note);
        // PulseLight(e.velocity / 127f);
        // FireProjectile(e.note % 12);
        if(e.note > 2) OnSaxNotePlayed.Raise(this, e);

        if (e.note == 1) OnSirenSplashAttack.Raise(this, e);

        if (e.note == 0) OnSirenWaveAttack.Raise(this, e);
    }
}

public struct MidiNoteEvent
{
    public double time;
    public int note;
    public int velocity;
}


    