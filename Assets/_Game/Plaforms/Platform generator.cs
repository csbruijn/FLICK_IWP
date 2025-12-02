using UnityEngine;

public class Platformgenerator : MonoBehaviour
{
    [SerializeField] private Transform minPos, maxPos;

    private float totalDist;

    private void Awake()
    {
        totalDist = maxPos.position.y - minPos.position.y;
        Debug.Log(totalDist); 
    }



}
