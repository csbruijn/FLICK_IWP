using TMPro;
using UnityEngine;

public class CountDownView : MonoBehaviour
{

    //i handle the countdown updates here. updates every frame for now

    [SerializeField] private GameEvent onCountDownChanged;
    [SerializeField] private float startTime = 10f;

    private float currentTime;

    void Start()
    {
        currentTime = startTime;
    }

    void Update()
    {
        if (currentTime <= 0f) return;

        currentTime -= Time.deltaTime;
        onCountDownChanged.Raise(this, currentTime);
    }

    //the time is in seconds now
    public void OnCountDownChanged(Component sender, System.Object data)
    {
        float timeToDisplay = (float)data;

        GetComponent<TextMeshProUGUI>().text = Mathf.CeilToInt(timeToDisplay).ToString();

        //Debug.Log("countdown script is working");
    }


}
