using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public static LevelsManager instance { get; private set; }

    public PlayData playData;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }


    public void GetMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GetGame()
    {
        //playData.outcome = GameOutcome.Conductor; 
        SceneManager.LoadScene(1);
    }

    public void GetPostGame()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitToDesktop()
    {
        Application.Quit();
    }


    public void OnGameOver(Component sender, System.Object data)
    {
        GameStats stats = (GameStats)data;

        playData.GameWon = stats.FGamewon; 
        playData.timeToCompletion = stats.FinalTimePlayed;
        playData.NotesCollected = stats.FinalNotesCollected; 
        
        GetPostGame();

    }



}
