using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Gamemanager;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance { get; private set; }

    public bool GameStarted { get; private set; } = false;
    public bool GameOver { get; private set; } = false; 

    public int NotesCollected { get; private set; }
    private int NotesToCollect;

    [SerializeField] public float scrollspeed { get; private set; } = 0.05f;
    public float scrolldist { get; private set; } = 0f;
    public float interval { get; private set; } = 1;
    [SerializeField] private int intervalDist = 1;

    [SerializeField] private GameEvent OnGameOver, OnInterValReached;


    bool outcomeSet = false;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

    }

    private void Start()
    {
        GameStarted = true;

    }

    private void FixedUpdate()
    {
        if (!GameStarted) return;

        scrolldist += scrollspeed;
        if (scrolldist> interval*intervalDist +1) 
        {
            interval += intervalDist; 
            OnInterValReached.Raise(this, interval -1);
        }
    }



    public void NoteCollected(Component sender, System.Object data)
    {

    }

    private void InitiateGameOver()
    {
       GameOver = true;
        Debug.Log("game over");
        OnGameOver.Raise(this, false);

    }

    

    public void SetOutcome(GameOutcome outcome)
    {
        if (outcomeSet) return;

        LevelsManager.instance.playData.outcome = outcome;
        outcomeSet = true;

    }    
}

public enum GameOutcome
{
    Conductor,
    Percussion,
    Brass,
    Woodwinds,
    strings
}