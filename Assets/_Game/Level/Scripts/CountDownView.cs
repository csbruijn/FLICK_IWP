using TMPro;
using UnityEngine;

public class CountDownView : MonoBehaviour
{
    //the time is in seconds now
    public void OnCountDownChanged(Component sender, System.Object data)
    {
        float timeToDisplay = (float)data;

        GetComponent<TextMeshProUGUI>().text = Mathf.CeilToInt(timeToDisplay).ToString();

        //Debug.Log("countdown script is working");
    }


}
