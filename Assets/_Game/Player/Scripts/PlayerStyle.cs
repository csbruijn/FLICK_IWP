using System.Collections.Generic;
using UnityEngine;

public class PlayerStyle : MonoBehaviour
{
    [SerializeField] private GameObject AliveVis, SpiritVis;

    [SerializeField] private SpriteRenderer alive, spirit; 

    public void InitializeStyle(PlayerStyleData style)
    {
        alive.sprite = style.alive;
        spirit.sprite = style.spirit;
        SetAliveStyle();
    }


    public void SetAliveStyle()
    {
        AliveVis.SetActive(true);
        SpiritVis.SetActive(false);
    }

    public void SetSpiritStyle()
    {
        AliveVis.SetActive(false);
        SpiritVis.SetActive(true);
    }
}
