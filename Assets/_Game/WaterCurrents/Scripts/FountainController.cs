using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class FountainController : MonoBehaviour
{
    private float fountainHeight = 5f;

    ParticleSystem ps;
    ParticleSystem.MainModule main;

    public List<ParticleCollisionEvent> collisionEvents;

    private List<ParticleSystem.Particle> inside = new();

    private PlayerStatus[] players; 
    

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        main = ps.main;
        collisionEvents = new List<ParticleCollisionEvent>(); 
    }

    public void OnGameStarted()
    {
        players = Gamemanager.instance.players;

        //Debug.Log(p.Length);
        //int i = 0; 
        //foreach (PlayerStatus pls in p)
        //{
        //    ps.trigger.SetCollider(i, pls.transform /*pls.GetComponent<CapsuleCollider2D>()*/);  
        //    i++;
        //}
    }

    public void SetHeight(float height)
    {
        main.startSpeed = height;
        fountainHeight = height;
    }

    private void OnParticleCollision(GameObject other)
    {
        //int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);

        other.GetComponent<PlayerController>().ApplyAirBoost(fountainHeight); 
    }

    private void OnParticleTrigger()
    {
        int count = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

        if (count == 0) return; 


        foreach (PlayerStatus ps in players)
        {
            Collider2D col = ps.GetComponent<Collider2D>();

            if (col == null) continue; 
            
            for (int i = 0; i < count; i++)
            {
                Vector2 particlePost = inside[i].position; 

                if (col.OverlapPoint(particlePost))
                {
                    ps.GetComponent<PlayerController>().ApplyAirBoost(fountainHeight);
                    break;
                }
            }
        }
    }

    
}