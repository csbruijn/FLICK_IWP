using UnityEngine;

public class FountainController : MonoBehaviour
{
    //public float fountainHeight = 5f;

    ParticleSystem ps;
    ParticleSystem.MainModule main;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        main = ps.main;
    }

    public void SetHeight(float height)
    {
        main.startSpeed = height;

    }
}