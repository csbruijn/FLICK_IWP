using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public bool isFinished = false;
    public bool isDead { get; private set; } = false;

    [SerializeField] private GameObject AliveVis, SpiritVis;

    private void Awake()
    {
        Gamemanager.instance.AddPlayerToList(this);
    }
    public void KillPlayer()
    {
        isDead = true;
        AliveVis.SetActive(false);
        SpiritVis.SetActive(true);
    }


    public void RevivePlayer()
    { 
        isDead = false;
        AliveVis.SetActive(true);
        SpiritVis.SetActive(false);

        // put player in position of the otherplayer 1 sec ago? 
    }
}
