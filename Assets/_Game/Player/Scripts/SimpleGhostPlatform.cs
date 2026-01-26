using UnityEngine;

public class SimpleGhostPlatform : MonoBehaviour
{
    [SerializeField] private GameObject platformRef;

    private void Start()
    {
        platformRef.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        platformRef.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)

    {
        if (!collision.CompareTag("Player")) return;

        platformRef.SetActive(false);

    }
}

