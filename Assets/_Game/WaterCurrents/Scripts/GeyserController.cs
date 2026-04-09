using System.Collections.Generic;
using UnityEngine;

public class GeyserController : MonoBehaviour
{
    public GameObject headPrefab;
    public GameObject bodyPrefab;

    public float tileHeight = 1f;
    public float growSpeed = 5f;
    public float maxHeight = 10f;

    private float currentHeight = 0f;

    private List<GameObject> bodyTiles = new();
    private GameObject head;


    [SerializeField] private BoxCollider2D triggerZone;

    void Start()
    {
        head = Instantiate(headPrefab, transform);
    }

    void Update()
    {
        Grow();
    }

    void Grow()
    {
        if (currentHeight >= maxHeight) return;

        currentHeight += growSpeed * Time.deltaTime;

        int neededTiles = Mathf.FloorToInt(currentHeight / tileHeight);

        // spawn missing tiles
        while (bodyTiles.Count < neededTiles)
        {
            SpawnTile();
        }

        UpdatePositions();



        void UpdateCollider()
        {
            float height = bodyTiles.Count * tileHeight;

            triggerZone.size = new Vector2(triggerZone.size.x, height);
            triggerZone.offset = new Vector2(0, height / 2f);
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ApplyAirBoost(maxHeight);
        }
    }

    void SpawnTile()
    {
        GameObject tile = Instantiate(bodyPrefab, transform);
        bodyTiles.Add(tile);
    }

    void UpdatePositions()
    {
        // position body tiles
        for (int i = 0; i < bodyTiles.Count; i++)
        {
            bodyTiles[i].transform.localPosition = new Vector3(0, i * tileHeight, 0);
        }

        // position head on top
        if (head != null)
        {
            head.transform.localPosition = new Vector3(0, bodyTiles.Count * tileHeight, 0);
        }
    }
}