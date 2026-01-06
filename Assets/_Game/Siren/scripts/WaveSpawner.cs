using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Wave setup")]
    public GameObject wavePrefab;
    public int waveCount = 5;
    public float horizontalSpacing = 0.5f;
    public float verticalSpacing = 0.5f;

    [Header("Spawn position")]
    public Transform spawnPoint;        // where waves start spawning

    [Header("Movement")]
    public float waveSpeed = 2f;
    public float lifeTime = 5f;         //when they die (should be more natural)

    // call this from button/from whatever triggers the attack
    public void SpawnWaveAttack()
    {
        if (!Gamemanager.instance.GameStarted) return;

        for (int i = 0; i < waveCount; i++)
        {
            Vector3 offset = new Vector3(i * horizontalSpacing, 0f, 0f);
            GameObject wave = Instantiate(
                wavePrefab,
                spawnPoint.position + offset,
                Quaternion.identity
            );


            // movement functions in another script 
            WaveMover mover = wave.AddComponent<WaveMover>();
            mover.waveSpeed = waveSpeed;
            mover.lifeTime = lifeTime;
        }
    }

    public void SpawnWaveAttackUp()
    {
        if (!Gamemanager.instance.GameStarted) return;

        for (int i = 0; i < waveCount; i++)
        {
            Vector3 offset = new Vector3(0f, i * verticalSpacing, 0f);
            GameObject wave = Instantiate(
                wavePrefab,
                spawnPoint.position + offset,
                Quaternion.identity
            );


            // movement functions in another script 
            WaveMoverUp mover = wave.AddComponent<WaveMoverUp>();
            mover.waveSpeed = waveSpeed;
            mover.lifeTime = lifeTime;
        }
    }

}


