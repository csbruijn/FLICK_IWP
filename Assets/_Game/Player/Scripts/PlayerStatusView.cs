using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusView : MonoBehaviour
{
    [Header("Data")]
    private Sprite IconAlive;
    private Sprite IconSpirit; 

    [Header("Refs")]
    [SerializeField] private TextMeshProUGUI playerNumber;
    [SerializeField] private Image PlayerIcon;

    [SerializeField] private GameObject heartContentView;
    [SerializeField] private GameObject HeartIcon;

    public void UpdateHPView(int newHP)
    {
        foreach(Transform child in heartContentView.transform)
        {
            Destroy(child.gameObject);
        }
        for(int i= 0; i < newHP; i++)
        {
            Instantiate(HeartIcon).transform.SetParent(heartContentView.transform,false);
        }

    }

    public void initilizeView(PlayerStyleData data)
    {
        IconAlive = data.alive;
        IconSpirit = data.spirit;
        playerNumber.text = data.playerID.ToString();

        PlayerIcon.sprite = data.instrument; 
    }
}
