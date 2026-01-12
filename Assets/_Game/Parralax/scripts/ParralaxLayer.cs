using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("Parallax Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float parallaxStrength = 0.3f;
    // Smaller = further away

    private Transform cam;
    private Vector3 lastCamPosition;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPosition = cam.position;
    }

    void LateUpdate()
    {
        Vector3 camDelta = cam.position - lastCamPosition;

        transform.position -= new Vector3(
            camDelta.x * parallaxStrength,
            camDelta.y * parallaxStrength,
            0f
        );

        lastCamPosition = cam.position;
    }
}
