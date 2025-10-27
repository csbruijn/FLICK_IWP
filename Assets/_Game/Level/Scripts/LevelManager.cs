using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }

    public bool GameStarted { get; private set; } = false;
    public bool GameOver { get; private set; } = false; 

    public int NotesCollected { get; private set; }
    private int NotesToCollect; 

    public float TimeToCompleteLevel { get; private set; } = 120;

    private float Remainingtime;

    [SerializeField] private GameEvent OnCountdownChanged, OnGameOVer;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        NotesToCollect = FindObjectsByType<ObjectiveNote>(FindObjectsSortMode.None).Length;
    }

    public void OnGameStarted(Component sender, System.Object data)
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
}
