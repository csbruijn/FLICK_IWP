using Unity.VisualScripting;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("Parallax Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float parallaxStrength = 0.3f;
    // Smaller = further away

    private Transform cam;
    private Vector3 lastCamPosition;

    [SerializeField] private GameObject respawnPoint;
    [SerializeField] private GameObject repeater;


    public bool AutoRepeat = true;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPosition = cam.position;

        GameObject[] repeaters = GameObject.FindGameObjectsWithTag("Repeater");
        repeater = repeaters[0];

        GameObject[] respawners = GameObject.FindGameObjectsWithTag("Respawner");
        respawnPoint = respawners[0];
    }

    void LateUpdate()
    {
        HandleParralax();
        HandleRepetition();
    }

    private void HandleParralax()
    {
        Vector3 camDelta = cam.position - lastCamPosition;

        transform.position -= new Vector3(
            camDelta.x * parallaxStrength,
            camDelta.y * parallaxStrength,
            0f
        );

        lastCamPosition = cam.position;
    }

    private void HandleRepetition()
    {
        if (!AutoRepeat) return;

        if (transform.position.x < repeater.transform.position.x) // if behind repeater go to respawnPoint
        {
            transform.position = new Vector3(
                        respawnPoint.transform.position.x,
                        transform.position.y,
                        transform.position.z
                        ); // DO SAID THING

            // switch out model if its 3D check if script is on the object and then this one checks if it has it
            ModelSwitcher ms = GetComponent<ModelSwitcher>();
            if (ms != null)
            {
              ms.switchOut();
            }
        }
    }
}


