using FMODUnity;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{
    private StudioEventEmitter emitter;
    [SerializeField, Tooltip("if between 0 and 2 it will select the corresponding track: 0=IDK, 1=TW, 2=GS")] 
    int track = -1;


    [Header("MIDI")]
    //[SerializeField] private string midiFileName = "basic_pitch_transcription(1).mid";

    private MidiFile midiFile;
    private TempoMap tempoMap;
    private List<MidiNoteEvent> events;

    private float songStartTime;
    private int index;
    private bool songPlaying;

    [SerializeField] private string IDK_MidifilePath;
    [SerializeField] private string TW_MidifilePath;
    [SerializeField] private string GS_MidifilePath;

    [Header("Events")]
    [SerializeField] private GameEvent OnSaxNotePlayed;
    [SerializeField] private GameEvent OnSirenWaveAttack;
    [SerializeField] private GameEvent OnSirenSplashAttack;
    [SerializeField] private GameEvent OnEndNotePlayed;

    [Header("Events")]
    int song;    

    private void Awake()
    {
        emitter = GetComponent<StudioEventEmitter>();

        if (track >= 0 && track <= 2) song = track; //predetermine for testing purposes
        else song = ChooseTrack();

        Debug.Log($"chose track no {song}");
        LoadMidi(song);
        InitTrack(song);
    }

    private int ChooseTrack()
    {
        int random; 
        return Random.Range(0, 3);
    }

    private void LoadMidi(int song)
    {
        switch (song)
        {
            case 0: midiFile = MidiFile.Read(IDK_MidifilePath); Debug.Log("MIDI : IDK"); break;
            case 1: midiFile = MidiFile.Read(TW_MidifilePath); Debug.Log("MIDI : TW"); break;
            case 2: midiFile = MidiFile.Read(GS_MidifilePath); Debug.Log("MIDI : GS"); break;
            default: midiFile = MidiFile.Read(IDK_MidifilePath); Debug.LogWarning("track out of bounds; falling back to 0"); break;
        }
        tempoMap = midiFile.GetTempoMap();
    }

    private void InitTrack(int song)
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
        emitter.Play();
        emitter.SetParameter("Tracks", song);

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
        if(e.note > 2) OnSaxNotePlayed.Raise(this, e);
        if (e.note == 2) OnEndNotePlayed.Raise(this, e);   
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


    