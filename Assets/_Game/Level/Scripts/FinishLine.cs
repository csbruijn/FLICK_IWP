using UnityEngine;

public class FinishLine : MonoBehaviour
{
    int playersFinished =0;
    [SerializeField] private GameEvent OnFinish;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStatus player = collision.GetComponent<PlayerStatus>();
        if (player == null) {
            Debug.Log("object is not player"); 
            return; } 
        
        if (player.isFinished) return;

        player.isFinished = true;

        playersFinished++;

        if (playersFinished >= Gamemanager.instance.totalPlayers)
        {
            OnFinish.Raise(this, null); 
        }
        
    }
}
