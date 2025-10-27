using TMPro;
using UnityEngine;

public class CountDownView : MonoBehaviour
{
    public void OnCountDownChanged(Component sender, System.Object data)
    {
        float timeToDisplay = (float)data;

        GetComponent<TextMeshProUGUI>().text = timeToDisplay.ToString();
    }
}
