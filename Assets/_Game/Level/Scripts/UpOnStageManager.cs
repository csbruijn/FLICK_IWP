using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UpOnStageManager;

public class UpOnStageManager : MonoBehaviour
{
    public static UpOnStageManager instance { get; private set; }

    public bool GameStarted { get; private set; } = false;
    public bool GameOver { get; private set; } = false; 

    public int NotesCollected { get; private set; }
    private int NotesToCollect; 

     public float TimeToCompleteLevel  = 120;

    public  float Remainingtime { get; private set; }

    [SerializeField] private GameEvent OnCountdownChanged, OnGameOVer;

    bool outcomeSet = false;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        NotesToCollect = FindObjectsByType<ObjectiveNote>(FindObjectsSortMode.None).Length;
    }

    private void Start()
    {
        GameStarted = true;

        Remainingtime = TimeToCompleteLevel;
        StartCoroutine(Countdown());
    }


    private IEnumerator Countdown()
    {
        while (Remainingtime > 0f)
        {
            yield return new WaitForSeconds(1f);
            Remainingtime -= 1f;
            OnCountdownChanged.Raise(this, Remainingtime);
        }
        
        InitiateGameOver();
    }

    public void NoteCollected(Component sender, System.Object data)
    {
        GameOutcome outcome = (GameOutcome)data;  
        
        SetOutcome(outcome); 

        NotesCollected++; 
        if (NotesCollected >= NotesToCollect)
        {
            GameOver = true;
            OnGameOVer.Raise(this, true);
        }
    }

    private void InitiateGameOver()
    {
       GameOver = true;
        Debug.Log("game over");
        OnGameOVer.Raise(this, false);

    }

    public void OnSceneReset()
    {
        SceneManager.LoadScene(0);
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