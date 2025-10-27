using UnityEngine;

public class StartButtonView : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    [SerializeField] private GameEvent OnGameStarted;
    
    public void SendStartGameRequest()
    {
        OnGameStarted.Raise(this, null);

        this.gameObject.SetActive(false);
    }
}
