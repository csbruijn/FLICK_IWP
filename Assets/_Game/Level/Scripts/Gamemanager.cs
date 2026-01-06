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
    int playersConnected = 0; 

    [SerializeField] private GameEvent OnGameOVer;
    [SerializeField] private GameEvent OnGameStarted;
    [SerializeField] private GameEvent onCountDownChanged;

    [SerializeField] private float startTime = 10f;

    private float currentTime;

    bool outcomeSet = false;

    public PlayerStatus[] players; 

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        currentTime = startTime;
        totalPlayers = GetComponent<UnityEngine.InputSystem.PlayerInputManager>().maxPlayerCount;
        players = new PlayerStatus[totalPlayers];
    }
    void Update()
    {
        if (!GameStarted) return;

        if (!CheckPlayersAlive()) OnGameOver();

        if (currentTime <= 0f) return;

        currentTime -= Time.deltaTime;
        onCountDownChanged.Raise(this, currentTime);
    }

    private bool CheckPlayersAlive()
    {
        foreach (PlayerStatus player in players)
        {
            if (!player.isDead) return true;   
        }
        return false;
    }

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

    public void AddPlayerToList(PlayerStatus player)
    {
        players[playersConnected-1] = player;
    }

    public void NoteCollected(Component sender, System.Object data)
    {

    }
    public void OnGameOver()
    {
        GameOver = true;
        Debug.Log("game lost");
        OnGameOVer.Raise(this, false);
    }
    public void OnGameFinish(Component sender, System.Object data)
    {
        GameOver = true;
        Debug.Log("game Won");
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