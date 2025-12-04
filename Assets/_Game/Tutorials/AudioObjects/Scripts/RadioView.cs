using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace FmodTesting
{
    public class RadioView : MonoBehaviour
    {
        private float volume = 0f;
        private float frequency = 0f;

        private void Awake()
        {
            DataChanged();
        }

        public void SetVol(float vol)
        {
            if (vol < 0f) volume = 0f;
            else if (vol > 1f) volume = 1f;
            else volume = vol;

            DataChanged();
        }

        public void SetFreq(float freq)
        {
            if (freq < 0f) frequency = 0f;
            else if (freq > 3f) frequency = 3f;
            else frequency = freq;

            DataChanged();
        }

        public GameEvent gameEvent;

        public void DataChanged()
        {
            RadioData radiodata = new RadioData(frequency, volume);
            gameEvent.Raise(this, radiodata);

            Debug.Log($"Send out F{frequency} + V{volume}");
        }
    }
}