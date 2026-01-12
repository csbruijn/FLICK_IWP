using UnityEngine;

using UnityEngine;
using UnityEngine.SceneManagement;

public class CountdownResetOnRestart : MonoBehaviour
{
    [SerializeField] private CountDownView countDownView;

    private void Start()
    {
        // When the scene starts, reset the UI visuals
        if (countDownView != null)
            countDownView.ResetCountdownVisual();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
