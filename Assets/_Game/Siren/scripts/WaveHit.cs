using UnityEngine;

public class WaveHit : MonoBehaviour
{
    public GameEvent OnPlayerHit; // where is our prepared event

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //idk ill do tags hopefully this time works 
        {
            OnPlayerHit.Raise(this, null);
        }
    }
}

