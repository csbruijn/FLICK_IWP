using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] ScrollerPlatform platformPrefab;

    List<ScrollerPlatform> platforms = new List<ScrollerPlatform>();

     

    [SerializeField] int maxPlatforms = 10; 

    
    
    float spawnX = 11,spawnYmin =0 , spawnYmax =14;   
    
    public void InstantiatePlatform(Component sender, object data)
    {
        float spawnY = (float)data; 
        //Instantiate(platformPrefab, new Vector3(spawnX, Random.RandomRange(spawnYmin,spawnYmax),0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(spawnX, spawnY, 0f), Quaternion.identity);

    }
}
