using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class NotesBonusController : MonoBehaviour
{
    private int TotalNotesToFulfill; 
    private float currentNotes = 0;

    [SerializeField] private GameEvent OnUpdateNotesBar;

    Gamemanager gm; 

    private void Start()
    {
        OnUpdateNotesBar.Raise(this, 0f);
        gm = Gamemanager.instance;

        TotalNotesToFulfill = gm.notesToFullBar;
    }

    public void OnNoteCollected(Component sender, System.Object data)
    {
        currentNotes+= (float)data;
        Debug.Log($"notes picked up: {currentNotes}");  
        if (currentNotes >= TotalNotesToFulfill)
        {            
            if(gm.AnyPlayerDead())
            {
                List<PlayerStatus> ps;
                ps = gm.GetDeadPlayers();
                {
                    //revive
                    foreach (PlayerStatus p in ps)
                    {
                        p.RevivePlayer();
                    }
                }
            }
            else
            {
                Debug.Log("healPlayers");
                foreach(PlayerStatus p in gm.players)
                {
                    p.HealPlayer();
                }

            }
            currentNotes = 0; 
        }
        float fillmount = (float)currentNotes / (float)TotalNotesToFulfill;
        Debug.Log($"progress: {fillmount}"); 
        OnUpdateNotesBar.Raise(this, fillmount);
    }
}
