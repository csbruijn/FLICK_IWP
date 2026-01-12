using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float parallaxFactor = 0.5f;
    // 0 = locked to camera, 1 = moves like normal world, smaller = "further away"

    [SerializeField] private Transform cameraTransform;

    private Vector3 lastCamPos;

    void Start()
    {
        if (!cameraTransform) cameraTransform = Camera.main.transform;
        lastCamPos = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 camDelta = cameraTransform.position - lastCamPos;
        transform.position += new Vector3(camDelta.x * parallaxFactor, camDelta.y * parallaxFactor, 0f);
        lastCamPos = cameraTransform.position;
    }
}
