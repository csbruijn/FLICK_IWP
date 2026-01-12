using FMODUnity;
using System.Collections;
using UnityEngine;

public class FmodFadeout : MonoBehaviour
{
    [Header("FMOD")]
    [SerializeField] private StudioEventEmitter musicEmitter;

    [Header("Fade Settings")]
    [SerializeField] private float fadeOutSpeed = 1f;
    [SerializeField] private string volumeParameter = "JazzVolume";

  public void StartFadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        float time = 1f;

        while (time > 0f)
        {
            float volume = time; 
            time -= Time.deltaTime * fadeOutSpeed;
            musicEmitter.SetParameter(volumeParameter, volume);

            yield return null;
        }

        musicEmitter.SetParameter(volumeParameter, 0f);
        musicEmitter.Stop();
    }
}
