using System.Collections;
using UnityEngine;

public class WaterSplashAttack : MonoBehaviour
{
    [SerializeField] private GameObject dropletPrefab;

    [Header("Burst")]
    [SerializeField] private int dropletCount = 22;
    [SerializeField] private float timeBetween = 0.02f;

    [Header("Arc")]
    [SerializeField] private float minAngle = 15f, maxAngle = 65f; // degrees
    [SerializeField] private float minSpeed = 8f, maxSpeed = 14f;

    [Header("Cleanup")]
    [SerializeField] private float destroySelfAfter = 2f;

    private void OnEnable()
    {
        StartCoroutine(Fire());
        Destroy(gameObject, destroySelfAfter);
    }

    private IEnumerator Fire()
    {
        for (int i = 0; i < dropletCount; i++)
        {
            SpawnDroplet();
            if (timeBetween > 0f) yield return new WaitForSeconds(timeBetween);
        }
    }

    private void SpawnDroplet()
    {
        if (!dropletPrefab) return;

        GameObject d = Instantiate(dropletPrefab, transform.position, Quaternion.identity);

        float a = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;
        float s = Random.Range(minSpeed, maxSpeed);

        Rigidbody2D rb = d.GetComponent<Rigidbody2D>();
        if (rb) rb.linearVelocity = new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * s; // always right
    }
}
