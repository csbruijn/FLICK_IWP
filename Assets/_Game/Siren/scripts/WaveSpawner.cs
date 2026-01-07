using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Wave setup")]
    [SerializeField] private GameObject wavePrefab;
    [SerializeField] private int waveCount = 5;
    [SerializeField] private float horizontalSpacing = 0.5f;
    [SerializeField] private float verticalSpacing = 0.5f;

    [Header("Spawn position")]
    [SerializeField] private Transform spawnPoint;        // where waves start spawning

    [Header("Movement")]
    [SerializeField] private float waveSpeed = 2f;
    [SerializeField] private float lifeTime = 5f;         //when they die (should be more natural)

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


