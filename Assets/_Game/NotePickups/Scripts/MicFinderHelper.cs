//using UnityEngine;
//using FMODUnity;
//using FMOD;
//using System;
//using System.Runtime.InteropServices;

//public class FMODMicrophone : MonoBehaviour
//{
//    [Header("Mic Settings")]
//    public int micDeviceIndex = 0;

//    public FMOD.Sound MicSound { get; private set; }
//    public FMOD.Channel MicChannel { get; private set; }
//    public FMOD.ChannelGroup MicChannelGroup { get; private set; }

//    private bool initialized = false;

//    void Start()
//    {
//        InitializeMicrophone();
//    }

//    public bool IsInitialized()
//    {
//        return initialized;
//    }

//    public void InitializeMicrophone()
//    {
//        if (initialized)
//            return;

//        FMOD.System coreSystem = RuntimeManager.CoreSystem;

//        // 1. Get mic device count
//        int numDrivers;
//        int numConnected;
//        coreSystem.getRecordNumDrivers(out numDrivers, out numConnected);

//        if (micDeviceIndex >= numDrivers)
//        {
//            UnityEngine.Debug.LogError("FMODMic: Mic index out of range. Found " + numDrivers + " devices.");
//            return;
//        }

//        // 2. Get device info
//        coreSystem.getRecordDriverInfo(
//            micDeviceIndex,
//            out string deviceName,
//            256,
//            out _,
//            out int sampleRate,
//            out FMOD.SPEAKERMODE speakerMode,
//            out int numChannels,
//            out FMOD.DRIVER_STATE state
//        );

//        UnityEngine.Debug.Log("FMODMic: Using device " + micDeviceIndex + " (" + deviceName + ")");

//        // 3. Configure streaming sound info
//        FMOD.CREATESOUNDEXINFO exInfo = new FMOD.CREATESOUNDEXINFO();
//        exInfo.cbsize = Marshal.SizeOf(typeof(FMOD.CREATESOUNDEXINFO));
//        exInfo.numchannels = numChannels;
//        exInfo.format = FMOD.SOUND_FORMAT.PCMFLOAT;
//        exInfo.defaultfrequency = sampleRate;
//        exInfo.length = (uint)(sampleRate * sizeof(float) * numChannels);

//        // 4. Create mic sound
//        coreSystem.createSound(
//            string.Empty,
//            FMOD.MODE.LOOP_NORMAL | FMOD.MODE.OPENUSER | FMOD.MODE.CREATESTREAM,
//            ref exInfo,
//            out MicSound
//        );

//        // 5. Start recording mic
//        coreSystem.recordStart(micDeviceIndex, MicSound, true);

//        // 6. Play the sound
//        coreSystem.playSound(
//            MicSound,
//            default(FMOD.ChannelGroup),
//            false,
//            out MicChannel
//        );

//        // 7. Capture channelgroup for DSP
//        MicChannel.getChannelGroup(out MicChannelGroup);

//        initialized = true;
//    }

//    public void StopMicrophone()
//    {
//        if (!initialized)
//            return;

//        RuntimeManager.CoreSystem.recordStop(micDeviceIndex);
//        MicChannel.stop();
//        initialized = false;
//    }

//    private void OnDestroy()
//    {
//        StopMicrophone();
//    }
//}
