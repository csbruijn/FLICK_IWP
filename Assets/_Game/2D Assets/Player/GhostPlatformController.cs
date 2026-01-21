using UnityEngine;

public class GhostPlatformController : MonoBehaviour
{
    //[SerializeField] private GameObject ghostPlatform;
    //[SerializeField] private float activationDistance = 1.3f;

    //private PlayerStatus selfStatus;
    //private PlayerController selfController;

    //void Awake()
    //{
    //    selfStatus = GetComponent<PlayerStatus>();
    //    selfController = GetComponent<PlayerController>();


    //    Debug.Log("GHOST PLATFORM AWAKE", this);


    //}

    //void Update()
    //{
        

    //    if (!selfStatus.isDead)
    //    {
    //        ghostPlatform.SetActive(false);
    //        return;
    //    }

    //    Debug.Log("I AM DEAD (GHOST)");

    //    int otherIndex = 1 - selfController.playerIndex;

    //    PlayerController other =
    //        Gamemanager.instance.playerControllers[otherIndex];

    //    if (other == null)
    //    {
    //        ghostPlatform.SetActive(false);
    //        return;
    //    }


    //    if (other.playerStatus.isDead)
    //    {
    //        Debug.Log("EXIT: other is dead");
    //        ghostPlatform.SetActive(false);
    //        return;
    //    }


    //    float dist = Vector2.Distance(transform.position, other.transform.position);
    //    Debug.Log("Distance to other for platform: " + dist);

    //    ghostPlatform.SetActive(dist < activationDistance);
    //}
}
