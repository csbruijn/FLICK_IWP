using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    private int playerID; 
    private Gamemanager gm; 

    public bool isFinished = false;
    public bool isDead { get; private set; } = false;

    [SerializeField] private int maxHP =4;
    private int HP; 

    [SerializeField] private GameEvent OnPlayerDamaged;

    [SerializeField] private PlayerStatusView playerStatusViewPrefab;
    private PlayerStatusView PlayerStatusView;

    public List<PlayerStyleData> styles = new();

    private void Awake()
    {
        HP = maxHP;

        gm = Gamemanager.instance;
        gm.AddPlayerToList(this);
        playerID = gm.playersConnected - 1;
        PlayerStatusView = Instantiate(playerStatusViewPrefab);
        PlayerStatusView.transform.SetParent(gm.playerUIContent.transform, false);
    }
        
        private void Start()
    {
        PlayerStatusView.initilizeView(styles[playerID]);
        PlayerStatusView.UpdateHPView(HP);

        GetComponent<PlayerStyle>().InitializeStyle(styles[playerID]);
    }


    

    public void DamagePlayer()
    {
        HP--;
        PlayerStatusView.UpdateHPView(HP);
        if (HP <= 0 ) KillPlayer();
    }

    public void HealPlayer()
    {
        if (HP < 4) HP++;
        PlayerStatusView.UpdateHPView(HP);
    }

    public void KillPlayer()
    {
        isDead = true;
        GetComponent<PlayerStyle>().SetSpiritStyle();

    }

    public void RevivePlayer()
    { 
        isDead = false;
        HP = 2;
        GetComponent<PlayerStyle>().SetAliveStyle();
        PlayerStatusView.UpdateHPView(HP);
        // put player in position of the otherplayer 1 sec ago? 
    }
}
