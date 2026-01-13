using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public static LevelsManager instance { get; private set; }

    public PlayData playData;

    public Animator backgroundAnimator;
    public CanvasGroup fadeOverlay;

    public float exitAnimationDuration = 1.2f;
    public float fadeDuration = 0.6f;

    [SerializeField] private float finishDelay = 5f;

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
        
        //this is to make sure animation runs before scene changes to main
        StartCoroutine(PlayExitFadeThenLoad());
       

        IEnumerator PlayExitFadeThenLoad()
        {
            backgroundAnimator.SetTrigger("StartGame");

            // start fading a bit after animation begins
            yield return new WaitForSeconds(exitAnimationDuration - fadeDuration);

            yield return StartCoroutine(FadeToBlack());

            SceneManager.LoadScene(1);
        }

        IEnumerator FadeToBlack()
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                fadeOverlay.alpha = Mathf.Clamp01(t / fadeDuration);
                yield return null;
            }

            fadeOverlay.alpha = 1f;
        }

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

        StartCoroutine(GameOverSequence()); 
    }

    private IEnumerator GameOverSequence()
    {

        yield return new WaitForSeconds(finishDelay);

        GetPostGame();

    }


}
