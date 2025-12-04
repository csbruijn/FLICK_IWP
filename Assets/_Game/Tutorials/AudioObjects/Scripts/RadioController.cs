using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using static FmodTesting.RadioView;
using UnityEngine.Rendering;

namespace FmodTesting
{


    public class RadioController : MonoBehaviour
    {
        private StudioEventEmitter emitter;

        void Awake()
        {
            emitter = GetComponent<StudioEventEmitter>();
        }

        public void SetParameter(Component sender, System.Object values)
        {
            if (values is not RadioData)
            {
                Debug.LogError($"ChangeCubeColour received invalid type: {values?.GetType().Name ?? "null"}");
                return;
            }

            RadioData radioData = (RadioData)values;
            Debug.Log($"recieved out F{radioData.freq} + V{radioData.vol}");

            emitter.SetParameter("Radio Volume", radioData.vol);
            emitter.SetParameter("Radio Frequency", radioData.freq);
        }
    }

    public struct RadioData
    {
        public float freq;
        public float vol;

        public RadioData(float F, float V)
        {
            this.freq = F;
            this.vol = V;
        }
    }
}