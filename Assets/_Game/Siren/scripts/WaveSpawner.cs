using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Wave setup")]
    [SerializeField] private GameObject wavePrefab;          // used by SpawnWaveAttack()
    [SerializeField] private GameObject waveUpPrefab;        // NEW: used by SpawnWaveAttackUp()

    [SerializeField] private int waveCount = 5;
    [SerializeField] private float horizontalSpacing = 0.5f;
    [SerializeField] private float verticalSpacing = 0.5f;

    [SerializeField] private float waveDelay =1, SplashDelay=1 ; 

    [Header("Spawn position")]
    [SerializeField] private Transform spawnPoint;

    [Header("Movement")]
    [SerializeField] private float waveSpeed = 2f;
    [SerializeField] private float lifeTime = 5f;

    public void SpawnWaveAttack()
    {
        if (!Gamemanager.instance.GameStarted) return;

        StartCoroutine(DelayedWaveAttackWave(waveDelay)); 
    }

    public void SpawnWaveAttackUp()
    {
        if (!Gamemanager.instance.GameStarted) return;

        StartCoroutine(DelayedWaveAttackUp(SplashDelay));

    }


    private IEnumerator DelayedWaveAttackUp(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // failsafe so nothing breaks if nothing is assigned in the inspector 
        GameObject prefabToUse = (waveUpPrefab != null) ? waveUpPrefab : wavePrefab;

        for (int i = 0; i < waveCount; i++)
        {
            Vector3 offset = new Vector3(0f, i * verticalSpacing, 0f);

            GameObject wave = Instantiate(
                prefabToUse,
                spawnPoint.position + offset,
                Quaternion.identity
            );

            // if this is a new splash spawner prefab, no need to add WaveMeverUp
            
            if (prefabToUse == wavePrefab)
            {
                WaveMoverUp mover = wave.AddComponent<WaveMoverUp>();
                mover.waveSpeed = waveSpeed;
                mover.lifeTime = lifeTime;
            }
        }
    }

    private IEnumerator DelayedWaveAttackWave(float delay)
    {
        yield return new WaitForSeconds(delay);

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
}
