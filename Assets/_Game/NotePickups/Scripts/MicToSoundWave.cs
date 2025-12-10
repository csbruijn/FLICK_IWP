using UnityEngine;
using FMODUnity;
using FMOD;
using System;
using System.Runtime.InteropServices;

public class MicToSoundWave : MonoBehaviour
{
    [Header("Microphone Settings")]
    public string preferredMicName = "";   // leave empty to use first mic
    public int micDeviceIndex = 0;         // fallback index

    private FMOD.Sound micSound;
    private FMOD.Channel micChannel;
    private FMOD.ChannelGroup channelGroup;
    private FMOD.DSP fftDsp;

    [Header("References")]
    public Transform[] bars;

    [Header("Settings")]
    public FrequencyFocusWindow frequencyFocusWindow = FrequencyFocusWindow.Entire;
    public float amplification = 1.0f;
    public float baseHeight = 0.0f;
    public bool useDecibels = false;

    [Header("State")]
    public float[] spectrumData;

    void Awake()
    {
        spectrumData = new float[4096];

        // Auto detect microphone
        micDeviceIndex = AutoSelectMicrophone();

        InitializeMicrophone();

        // Create FFT DSP
        RuntimeManager.CoreSystem.createDSPByType(FMOD.DSP_TYPE.FFT, out fftDsp);
        fftDsp.setParameterInt((int)FMOD.DSP_FFT.WINDOWSIZE, 4096);

        // Attach FFT DSP
        channelGroup.addDSP(0, fftDsp);
    }

    // ----------------------------------------------------------------------

    private int AutoSelectMicrophone()
    {
        FMOD.System core = RuntimeManager.CoreSystem;

        int numDrivers;
        int numConnected;
        core.getRecordNumDrivers(out numDrivers, out numConnected);

        UnityEngine.Debug.Log("FMOD: Found " + numDrivers + " recording devices.");

        // List all mics
        for (int i = 0; i < numDrivers; i++)
        {
            core.getRecordDriverInfo(
                i,
                out string name,
                256,
                out _,
                out int sampleRate,
                out FMOD.SPEAKERMODE speakermode,
                out int channels,
                out FMOD.DRIVER_STATE state
            );

            UnityEngine.Debug.Log("Mic " + i + ": " + name + " (" + sampleRate + " Hz, " + channels + " ch)");

            // Auto-match name
            if (!string.IsNullOrEmpty(preferredMicName))
            {
                if (name.ToLower().Contains(preferredMicName.ToLower()))
                {
                    UnityEngine.Debug.Log("Auto-selected mic: " + name);
                    return i;
                }
            }
        }

        // No match found: use first device (0)
        UnityEngine.Debug.Log("No preferred mic matched. Using default index 0.");
        return 0;
    }

    // ----------------------------------------------------------------------

    private void InitializeMicrophone()
    {
        FMOD.System core = RuntimeManager.CoreSystem;

        // Get device info
        core.getRecordDriverInfo(
            micDeviceIndex,
            out string deviceName,
            256,
            out _,
            out int sampleRate,
            out FMOD.SPEAKERMODE speakerMode,
            out int channels,
            out FMOD.DRIVER_STATE state
        );

        UnityEngine.Debug.Log("Initializing mic: " + deviceName + " at index " + micDeviceIndex);

        // Create sound buffer for microphone
        FMOD.CREATESOUNDEXINFO ex = new FMOD.CREATESOUNDEXINFO();
        ex.cbsize = Marshal.SizeOf(typeof(FMOD.CREATESOUNDEXINFO));
        ex.numchannels = channels;
        ex.format = FMOD.SOUND_FORMAT.PCMFLOAT;
        ex.defaultfrequency = sampleRate;

        // MUST be >0 or FMOD rejects recording
        ex.length = (uint)(sampleRate * sizeof(float) * channels);

        // Correct mode for microphone
        core.createSound(
            string.Empty,
            FMOD.MODE.LOOP_NORMAL |
            FMOD.MODE.OPENUSER |
            FMOD.MODE.CREATESAMPLE,
            ref ex,
            out micSound
        );

        // Start recording
        core.recordStart(micDeviceIndex, micSound, true);

        // Play the incoming audio
        core.playSound(
            micSound,
            default(FMOD.ChannelGroup),
            false,
            out micChannel
        );

        micChannel.getChannelGroup(out channelGroup);
    }


    // ----------------------------------------------------------------------

    void Update()
    {
        ReadFmodSpectrum();
        UpdateBars();
    }

    private void ReadFmodSpectrum()
    {
        IntPtr dataPtr;
        uint dataLen;

        fftDsp.getParameterData(2, out dataPtr, out dataLen);
        if (dataPtr == IntPtr.Zero)
            return;

        FMOD.DSP_PARAMETER_FFT fft = (FMOD.DSP_PARAMETER_FFT)
            Marshal.PtrToStructure(dataPtr, typeof(FMOD.DSP_PARAMETER_FFT));

        if (fft.numchannels == 0)
            return;

        float[] fmodData = fft.spectrum[0];
        if (fmodData == null)
            return;

        int count = Mathf.Min(fmodData.Length, spectrumData.Length);
        for (int i = 0; i < count; i++)
            spectrumData[i] = fmodData[i];
    }

    private void UpdateBars()
    {
        if (bars == null || bars.Length == 0)
            return;

        int blockSize = spectrumData.Length / bars.Length / (int)frequencyFocusWindow;

        for (int i = 0; i < bars.Length; i++)
        {
            float sum = 0f;
            int offset = i * blockSize;

            for (int j = 0; j < blockSize; j++)
                sum += spectrumData[offset + j];

            sum /= blockSize;

            float amplitude = Mathf.Clamp(sum, 1e-7f, 1f);

            Vector3 s = bars[i].localScale;

            if (useDecibels)
                s.y = -Mathf.Log10(amplitude) * amplification / 200f;
            else
                s.y = sum * amplification + baseHeight;

            bars[i].localScale = s;
        }
    }
}

// ----------------------------------------------------------------------

public enum FrequencyFocusWindow
{
    Entire = 1,
    FirstHalf = 2,
    FirstQuarter = 4,
    FirstEight = 8,
    FirstSixteenth = 16
}
