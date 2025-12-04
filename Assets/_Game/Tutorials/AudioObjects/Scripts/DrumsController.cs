using FMODUnity;
using UnityEngine;
namespace FmodTesting
{
    public class DrumsController : MonoBehaviour
    {

        private StudioEventEmitter studioEventEmitter;

        private Animator[] StickAnimators;

        void Awake()
        {
            studioEventEmitter = GetComponent<StudioEventEmitter>();
            StickAnimators = GetComponentsInChildren<Animator>();
            Debug.Log(StickAnimators.Length);
        }

        public void OnDrumPlayed()
        {
            Animator Stick = StickAnimators[0];


            if (Stick.GetCurrentAnimatorStateInfo(0).IsName("Resting"))
            {
                Stick.SetTrigger("Drum");
                studioEventEmitter.Play();
            }
            else
            {
                Stick = StickAnimators[1];

                if (Stick.GetCurrentAnimatorStateInfo(0).IsName("Resting"))
                {
                    Stick.SetTrigger("Drum");

                    studioEventEmitter.Play();
                }
            }
        }

    }
}