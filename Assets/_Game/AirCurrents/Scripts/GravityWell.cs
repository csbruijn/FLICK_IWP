using UnityEngine;

public class GravityWell : MonoBehaviour
{

    private float boostStrength = 5f;

    public void SetStrenght(float value)
    {
        boostStrength = value;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ApplyAirBoost(boostStrength);
        }
    }
 }
