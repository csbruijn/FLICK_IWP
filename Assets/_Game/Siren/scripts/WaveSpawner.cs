using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Wave setup")]
    [SerializeField] private GameObject wavePrefab;          // used by SpawnWaveAttack()
    [SerializeField] private GameObject waveUpPrefab;        // NEW: used by SpawnWaveAttackUp()

    [SerializeField] private int waveCount = 5;
    [SerializeField] private float horizontalSpacing = 0.5f;
    [SerializeField] private float verticalSpacing = 0.5f;

    [Header("Spawn position")]
    [SerializeField] private Transform spawnPoint;

    [Header("Movement")]
    [SerializeField] private float waveSpeed = 2f;
    [SerializeField] private float lifeTime = 5f;

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

            WaveMover mover = wave.AddComponent<WaveMover>();
            mover.waveSpeed = waveSpeed;
            mover.lifeTime = lifeTime;
        }
    }

    public void SpawnWaveAttackUp()
    {
        if (!Gamemanager.instance.GameStarted) return;

        // fallback so you don’t break anything if you forget to assign it
        GameObject prefabToUse = (waveUpPrefab != null) ? waveUpPrefab : wavePrefab;

        for (int i = 0; i < waveCount; i++)
        {
            Vector3 offset = new Vector3(0f, i * verticalSpacing, 0f);

            GameObject wave = Instantiate(
                prefabToUse,
                spawnPoint.position + offset,
                Quaternion.identity
            );

            // If this prefab is your new water splash spawner, it won’t need WaveMoverUp.
            // So only add WaveMoverUp if you're still using the old prefab.
            if (prefabToUse == wavePrefab)
            {
                WaveMoverUp mover = wave.AddComponent<WaveMoverUp>();
                mover.waveSpeed = waveSpeed;
                mover.lifeTime = lifeTime;
            }
        }
    }
}
