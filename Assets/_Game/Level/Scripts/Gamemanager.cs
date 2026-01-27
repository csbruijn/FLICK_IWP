using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Gamemanager;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance { get; private set; }

    // GameStatus
    public bool GameStarted { get; private set; } = false;
    public bool GameOver { get; private set; } = false;

    [Header("Game Rules")]
    [SerializeField] private float startScrollSpeed = 1.5f;
    public float currentScrollSpeed { get; private set; }

    [SerializeField] private float startTime = 10f;
    private float currentTime;

    public int notesToFullBar = 10;
    private int TotNotesCollected = 0;
    private float TimePlayed = 0f; 

    //[Header("PlayerCount")]
    public int totalPlayers { get; private set; }
    public int playersConnected { get; private set; } = 0;  
    public PlayerStatus[] players { get; private set; }

    //this is for ghost platform generation (needed to get components so playerstatus wasn't enough)
    public PlayerController[] playerControllers { get; private set; }


    [Header("GameEvents")]
    [SerializeField] private GameEvent OnGameOVer;
    [SerializeField] private GameEvent OnGameStarted;
    [SerializeField] private GameEvent onCountDownChanged;

    [Header("refs")]
    public Transform playerUIContent; 

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        currentTime = startTime;
        currentScrollSpeed = startScrollSpeed; 
        InitPlayerCount();
    }
    void Update()
    {
        if (!GameStarted) return;

        TimePlayed += Time.deltaTime;
        if (!CheckPlayersAlive()) OnGameOver();

        if (currentTime <= 0f) return;

        currentTime -= Time.deltaTime;
        onCountDownChanged.Raise(this, currentTime);
    }

    #region PlayerstatusChecks

    private bool CheckPlayersAlive()
    {
        foreach (PlayerStatus player in players)
        {
            if (!player.isDead) return true;   
        }
        return false;
    }

    public bool AnyPlayerDead()
    {
        foreach (PlayerStatus player in players)
        {
            if (player.isDead)
            {
                return true;
            }
        }
        return false;
    }

    public List<PlayerStatus> GetDeadPlayers()
    {
        List<PlayerStatus> ps = new();
        foreach (PlayerStatus player in players)
        {
            if (player.isDead)  ps.Add(player); 
           
        }
        return ps;
    }

    #endregion

    #region initialization
    public void OnPlayerJoined()
    {
        Debug.Log("player joined");

        playersConnected = GetComponent<UnityEngine.InputSystem.PlayerInputManager>().playerCount;



        if (playersConnected == totalPlayers)
        {
            Debug.Log("Start Game");
            GameStarted = true ;
            OnGameStarted.Raise(this, null); 
        }
    }
    private void InitPlayerCount()
    {
        totalPlayers = GetComponent<UnityEngine.InputSystem.PlayerInputManager>().maxPlayerCount;
        players = new PlayerStatus[totalPlayers];
        // again addition for ghost platform
        playerControllers = new PlayerController[totalPlayers];

    }

    public void AddPlayerToList(PlayerStatus player)
    {
        players[playersConnected-1] = player;
    }
    #endregion

    #region GameStatus
    public void OnGameOver()
    {
        GameOver = true;
        Debug.Log("game lost");
        GameStats stats = new GameStats(false, TimePlayed, TotNotesCollected);
        OnGameOVer.Raise(this, stats);
    }

    public void OnGameFinish(Component sender, System.Object data)
    {
        GameOver = true;
        Debug.Log("game Won");
        GameStats stats = new GameStats(true, TimePlayed, TotNotesCollected);
        OnGameOVer.Raise(this, stats);
    }

    private void SetPlayData()
    {
        LevelsManager.instance.playData.NotesCollected = TotNotesCollected;
        LevelsManager.instance.playData.timeToCompletion = TimePlayed;

    }

    //public void SetOutcome(GameOutcome outcome)
    //{
    //    if (outcomeSet) return;
    //    LevelsManager.instance.playData.outcome = outcome;
    //    outcomeSet = true;
    //}
    #endregion

    public void OnNoteCollected()
    {
            TotNotesCollected ++;
    }
}


public struct GameStats
{
    public bool FGamewon;
    public float FinalTimePlayed;
    public int FinalNotesCollected; 

    public GameStats(bool state, float time, int notes)
    {
        FGamewon = state;
        FinalTimePlayed = time;
        FinalNotesCollected = notes;
    }
 }

//public enum GameOutcome
//{
//    Conductor,
//    Percussion,
//    Brass,
//    Woodwinds,
//    strings
//}