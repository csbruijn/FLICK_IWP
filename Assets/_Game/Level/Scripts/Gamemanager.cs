using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Gamemanager;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance { get; private set; }

    public bool GameStarted { get; private set; } = false;
    public bool GameOver { get; private set; } = false; 

    public int NotesCollected { get; private set; }
    private int NotesToCollect;

    public float scrollspeed = 5f;

    public int totalPlayers;

    [SerializeField] private GameEvent OnGameOVer;

    bool outcomeSet = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void OnPlayerJoined()
    {
        Debug.Log("player joined");

        int playersConnected = GetComponent<PlayerInputManager>().playerCount;
        totalPlayers = GetComponent<PlayerInputManager>().maxPlayerCount;

        if (playersConnected == totalPlayers)
        {
            Debug.Log("Start Game");
            GameStarted = true ;
        }
    }

    public void NoteCollected(Component sender, System.Object data)
    {

    }

    public void OnGameFinish(Component sender, System.Object data)
    {
        GameOver = true;
        Debug.Log("game over");
        OnGameOVer.Raise(this, true);
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