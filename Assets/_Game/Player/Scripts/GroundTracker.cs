using UnityEngine;

public class GroundTracker : MonoBehaviour
{
    private KeyBoardPlatform keyBoardPlatform;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<KeyBoardPlatform>() != null)
        {
            keyBoardPlatform = collision.collider.GetComponent<KeyBoardPlatform>();
            transform.parent.SetParent(keyBoardPlatform.transform);
            Debug.Log("Attach");
        }
    }

}
