using System;
using Unity.Android.Gradle.Manifest;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public Transform[] spawnPostions; 
    private float[] bars;

    public ObjectiveNote coin;  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPostions = GetComponent<MicToSoundWave>().GetBars();
        bars = new float[GetComponent<MicToSoundWave>().GetBars().Length];
    }

    public void AddBarValue(int i, float v)
    {
       
        bars[i] += v;
        //Debug.Log(bars[i]);
    }

    public void SpawnNotes(Component sender, System.Object data)
    {

        float reachedInterval = (float)data;
        Debug.Log($"Interval reached: {reachedInterval}");

        // topIndices[0] = index of largest, topIndices[3] = index of 4th largest
        int[] Top = { -1, -1, -1, -1 };

        for (int i = 0; i < bars.Length; i++)
        {
            float num = bars[i];
            Debug.Log(num);

            if (Top[0] == -1 || num > bars[Top[0]])
            {
                Top[3] = Top[2];
                Top[2] = Top[1];
                Top[1] = Top[0];
                Top[0] = i;
            }
            else if (Top[1] == -1 || num > bars[Top[1]])
            {
                Top[3] = Top[2];
                Top[2] = Top[1];
                Top[1] = i;
            }
            else if (Top[2] == -1 || num > bars[Top[2]])
            {
                Top[3] = Top[2];
                Top[2] = i;
            }
            else if (Top[3] == -1 || num > bars[Top[3]])
            {
                Top[3] = i;
            }
        }

        
        foreach (int idx in Top)
        {
            Debug.Log($"spawn coin at: {idx}");
            Instantiate(coin, spawnPostions[idx].position, Quaternion.identity);
        }

        

        // reset
        for (int i = 0; i < bars.Length; i++) { bars[i] = 0; }
    }
}
