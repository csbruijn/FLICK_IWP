using System.Collections;
using UnityEngine;

public class ScrollerPlatform : MonoBehaviour
{
    public GameEvent OnPlatformEnd;

    public float xMinBorder;
    [SerializeField] float platformSpeed = .05f;

    private bool platformEnabled;

    private void Start()
    {
        if (!LevelManager.instance.GameStarted)  DisablePlatforms(); 
        else EnablePlatforms();
    }

    private void FixedUpdate()
    {
        if (!platformEnabled) return;
        
        // move the platform to the left 

        transform.position = new Vector3(transform.position.x - platformSpeed, transform.position.y, transform.position.z);
        
        // if far enough, remove platform 

        if (transform.position.x < xMinBorder) RemovePlatform();
    }

    private void RemovePlatform()
    {
        OnPlatformEnd.Raise(this, transform.position.y);
        Destroy(gameObject);
    }


    public void OnMusicNoteHit(Component sender, object data)
    {
        StartCoroutine(TempDisablePlatforms(1f));
    }

    public void OnGameStarted(Component sender, object data)
    {
        EnablePlatforms();
    }


    private IEnumerator TempDisablePlatforms(float timeToWait)
    {
        DisablePlatforms();

        //wait some time 
        yield return new WaitForSeconds(timeToWait);

        EnablePlatforms();
    }

    private void DisablePlatforms()
    {
        platformEnabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void EnablePlatforms()
    {
        platformEnabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }

}
