using TMPro;
using UnityEngine;

public class CountDownView : MonoBehaviour
{
    [SerializeField] private GameObject countdownPlate;

    private TextMeshProUGUI countdownText;

    private void Awake()
    {
        countdownText = GetComponent<TextMeshProUGUI>();
    }

  
    public void OnCountDownChanged(Component sender, System.Object data)
    {
        float timeToDisplay = (float)data;
        int seconds = Mathf.CeilToInt(timeToDisplay);

        countdownText.text = seconds.ToString();

        // Show plate while counting, hide at 0
        if (seconds <= 0)
            countdownPlate.SetActive(false);
        else
            countdownPlate.SetActive(true);
    }

    
    public void ResetCountdownVisual()
    {
        countdownPlate.SetActive(true);
        countdownText.text = "";
    }
}
