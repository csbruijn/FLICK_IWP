using UnityEngine;
namespace FmodTesting
{
public class DrumsView : MonoBehaviour
{
    public GameEvent OnDrumsPlayed;

    public void DrumsButtonPressed()
    {
        OnDrumsPlayed.Raise(this, null);
        Debug.Log("Play the drums");
    }
}
}
