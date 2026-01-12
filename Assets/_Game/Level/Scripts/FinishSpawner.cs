using UnityEngine;

public class FinishSpawner : MonoBehaviour
{
    [SerializeField] private FinishLine finishPrefab; 
    [SerializeField] private Transform spawnPos; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnEndNotePlayed(Component sender, System.Object Data)
    {
        Debug.Log("Spawn Finish");
        Instantiate(finishPrefab, spawnPos.position, Quaternion.identity);
    }
}
