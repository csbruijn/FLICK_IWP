using UnityEngine;

[CreateAssetMenu(fileName = "PlayData", menuName = "Scriptable Objects/PlayData")]
public class PlayData : ScriptableObject
{
    public bool GameWon; 
    //public GameOutcome outcome;
    public float timeToCompletion;
    public int NotesCollected;

}
