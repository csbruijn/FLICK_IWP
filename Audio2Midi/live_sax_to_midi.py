"""
Saxophone tuner using Harmonic Product Spectrum (HPS)
and sending pitch detection to a MIDI output port.

Requires:
    pip install sounddevice mido python-rtmidi numpy scipy
"""

import copy
import os
import numpy as np
import scipy.fftpack
import sounddevice as sd
import time
import mido

# -------------------------
# User settings
# -------------------------
SAMPLE_FREQ = 48000
WINDOW_SIZE = 48000
WINDOW_STEP = 12000
NUM_HPS = 5
POWER_THRESH = 1e-6
CONCERT_PITCH = 440
WHITE_NOISE_THRESH = .2

WINDOW_T_LEN = WINDOW_SIZE / SAMPLE_FREQ
DELTA_FREQ = SAMPLE_FREQ / WINDOW_SIZE
OCTAVE_BANDS = [50,100,200,400,800,1600,3200,6400,12800,25600]

MIDI_CHANNEL = 1      # 0–15
MIDI_PORT_NAME = "SaxToMidi 3"

ALL_NOTES = ["A","A#","B","C","C#","D","D#","E","F","F#","G","G#"]

def find_closest_note(pitch):
    i = int(np.round(np.log2(pitch/CONCERT_PITCH)*12))
    closest_note = ALL_NOTES[i % 12] + str(4 + (i + 9) // 12)
    closest_pitch = CONCERT_PITCH * 2**(i/12)
    midi_number = i + 69
    # clamp to MIDI range 0–127
    midi_number = max(0, min(127, midi_number))
    return closest_note, closest_pitch, midi_number


HANN_WINDOW = np.hanning(WINDOW_SIZE)

# -------------------------
# Set up MIDI output
# -------------------------
# Create a virtual port if OS supports it
try:
    midi_out = mido.open_output(MIDI_PORT_NAME)
except:
    # fallback: open first available port
    ports = mido.get_output_names()
    if not ports:
        raise RuntimeError("No MIDI output ports available!")
    midi_out = mido.open_output(ports[0])
    print("Using MIDI port:", ports[0])

last_midi_note = None

def send_midi_note(midi_number):
    global last_midi_note

    if midi_number == last_midi_note:
        return  # nothing to change

    # turn off previous
    if last_midi_note is not None:
        midi_out.send(mido.Message("note_off", note=last_midi_note,
                                   velocity=0, channel=MIDI_CHANNEL))

    # turn on new
    midi_out.send(mido.Message("note_on", note=midi_number,
                               velocity=100, channel=MIDI_CHANNEL))

    last_midi_note = midi_number


# -------------------------
# Audio callback
# -------------------------

def callback(indata, frames, time_info, status):
    if not hasattr(callback, "window_samples"):
        callback.window_samples = [0 for _ in range(WINDOW_SIZE)]
    if not hasattr(callback, "noteBuffer"):
        callback.noteBuffer = ["1","2"]

    if status:
        print(status)
        return

    if any(indata):
        callback.window_samples = np.concatenate((callback.window_samples, indata[:,0]))
        callback.window_samples = callback.window_samples[len(indata[:,0]):]

        signal_power = (np.linalg.norm(callback.window_samples)**2)/len(callback.window_samples)
        if signal_power < POWER_THRESH:
            print("Note: ...")
            return

        hann_samples = callback.window_samples * HANN_WINDOW
        magnitude_spec = abs(scipy.fftpack.fft(hann_samples)[:len(hann_samples)//2])

        # NO 62 Hz high-pass (sax can produce lower harmonics)

        # Noise suppression in octave bands
        for j in range(len(OCTAVE_BANDS)-1):
            start = int(OCTAVE_BANDS[j]/DELTA_FREQ)
            end   = int(OCTAVE_BANDS[j+1]/DELTA_FREQ)
            end = min(end, len(magnitude_spec))
            avg_energy = (np.linalg.norm(magnitude_spec[start:end])**2)/(end-start)
            avg_energy = avg_energy**0.5

            for i in range(start, end):
                if magnitude_spec[i] < WHITE_NOISE_THRESH * avg_energy:
                    magnitude_spec[i] = 0

        # Interpolation
        mag_ipol = np.interp(np.arange(0,len(magnitude_spec),1/NUM_HPS),
                              np.arange(0,len(magnitude_spec)),
                              magnitude_spec)
        mag_ipol = mag_ipol / np.linalg.norm(mag_ipol)

        hps = copy.deepcopy(mag_ipol)
        for i in range(NUM_HPS):
            tmp = np.multiply(hps[:int(np.ceil(len(mag_ipol)/(i+1)))], mag_ipol[::(i+1)])
            if not any(tmp):
                break
            hps = tmp

        max_ind = np.argmax(hps)
        max_freq = max_ind * (SAMPLE_FREQ/WINDOW_SIZE) / NUM_HPS

        closest_note, closest_pitch, midi_number = find_closest_note(max_freq)
        callback.noteBuffer.insert(0, closest_note)
        callback.noteBuffer.pop()

        if callback.noteBuffer.count(callback.noteBuffer[0]) == len(callback.noteBuffer):
            print(f"Note: {closest_note}  {max_freq:.1f}/{closest_pitch:.1f}  MIDI:{midi_number}")
            send_midi_note(midi_number)
        else:
            print("Note: ...")
    else:
        print("No input")


# -------------------------
# Run the tuner
# -------------------------
try:
    print("Starting Saxophone → MIDI tuner...")
    with sd.InputStream(channels=1, callback=callback,
                        blocksize=WINDOW_STEP, samplerate=SAMPLE_FREQ):
        while True:
            time.sleep(0.5)

except Exception as exc:
    print(str(exc))
