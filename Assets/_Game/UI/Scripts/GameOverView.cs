using TMPro;
using UnityEngine;

public class GameOverView : MonoBehaviour
{

    [SerializeField]private TextMeshProUGUI mText;
    [SerializeField] private GameObject GameoverButton;
    private void Awake()
    {
        //mText = GetComponent<TextMeshProUGUI>();
        mText.text = ""; 
    }

    public void OnGameOver(Component sender, System.Object data)
    {
        GameoverButton.SetActive(true);


        if ((bool)data)
            mText.text = "Game Won!";

        else
            mText.text = "Game Lost!";
         
    }

    public void OnResetPressed()
    {
        LevelManager.instance.OnSceneReset();
    }
}
