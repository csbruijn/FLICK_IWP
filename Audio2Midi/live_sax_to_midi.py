import math
import queue
import numpy as np
import sounddevice as sd
import aubio
import mido

mido.set_backend('mido.backends.rtmidi')  # ensure rtmidi backend

# --------- CONFIG ----------
SAMPLERATE = 44100
FRAMES_PER_BUFFER = 512
HOP_SIZE = FRAMES_PER_BUFFER
PITCH_METHOD = "yin"
SILENCE_RMS_THRESHOLD = 0.0025
STABLE_COUNT_FOR_ON = 3
STABLE_COUNT_FOR_OFF = 2
SMOOTHING_ALPHA = 0.3
MIDI_CHANNEL = 1
MIDI_OUT_NAME = "SaxToMidi 2"   # exact port name from loopMIDI
# ----------------------------

def freq_to_midi(freq):
    if freq <= 0:
        return None
    return int(round(69 + 12 * math.log2(freq / 440.0)))

def safe_midi_note(n):
    if n is None:
        return None
    return int(np.clip(n, 0, 127))

def amp_to_velocity(a):
    return int(np.clip((a / 0.2) ** 0.6 * 127, 1, 127))

def rms(samples):
    return float(np.sqrt(np.mean(samples * samples)))

# audio queue
audio_q = queue.Queue(maxsize=20)

def audio_callback(indata, frames, time_info, status):
    if status:
        print("[audio callback] status:", status)
    audio_q.put(indata[:, 0].copy())

def open_midi_output(port_name):
    available_ports = mido.get_output_names()
    if port_name not in available_ports:
        raise ValueError(f"MIDI port '{port_name}' not found. Available ports:\n{available_ports}")
    out = mido.open_output(port_name)
    print(f"[MIDI] Sending to port: {out.name}")
    return out

def main():
    print("Live sax → MIDI starting...")
    out = open_midi_output(MIDI_OUT_NAME)

    aubio_pitch = aubio.pitch(PITCH_METHOD, HOP_SIZE * 4, HOP_SIZE, SAMPLERATE)
    aubio_pitch.set_unit("Hz")
    aubio_pitch.set_silence(-40)

    smoothed_freq = 0.0
    last_note = None
    stable_count = 0
    silence_count = 0

    with sd.InputStream(channels=1, callback=audio_callback,
                        blocksize=FRAMES_PER_BUFFER, samplerate=SAMPLERATE):
        print("Microphone opened. Play your saxophone!")

        try:
            while True:
                try:
                    block = audio_q.get(timeout=1)
                except queue.Empty:
                    continue

                samples = block.astype(np.float32)
                amp = rms(samples)

                freq = float(aubio_pitch(samples)[0])
                if math.isnan(freq) or freq < 0:
                    freq = 0.0

                smoothed_freq = SMOOTHING_ALPHA * smoothed_freq + (1 - SMOOTHING_ALPHA) * freq

                if amp < SILENCE_RMS_THRESHOLD or smoothed_freq < 40:
                    silence_count += 1
                else:
                    silence_count = 0

                if 40 <= smoothed_freq <= 2000:
                    midi_note = freq_to_midi(smoothed_freq)
                else:
                    midi_note = None

                midi_note_safe = safe_midi_note(midi_note)

                # NOTE STATE
                if midi_note_safe is not None and silence_count == 0:
                    if midi_note_safe == last_note:
                        stable_count += 1
                    else:
                        stable_count += 1
                        if stable_count >= STABLE_COUNT_FOR_ON:
                            if last_note is not None:
                                out.send(mido.Message('note_off', note=last_note,
                                                      velocity=0, channel=MIDI_CHANNEL))
                            velocity = amp_to_velocity(amp)
                            out.send(mido.Message('note_on', note=midi_note_safe,
                                                  velocity=velocity, channel=MIDI_CHANNEL))
                            last_note = midi_note_safe
                            stable_count = 0
                else:
                    stable_count = 0
                    if last_note is not None and silence_count >= STABLE_COUNT_FOR_OFF:
                        out.send(mido.Message('note_off', note=last_note,
                                              velocity=0, channel=MIDI_CHANNEL))
                        last_note = None

                print(f"AMP={amp:.4f} FREQ={smoothed_freq:.1f} MIDI={midi_note_safe} LAST={last_note}", end='\r')

        except KeyboardInterrupt:
            print("\nInterrupted by user.")

        finally:
            if last_note is not None:
                out.send(mido.Message('note_off', note=last_note,
                                      velocity=0, channel=MIDI_CHANNEL))
            out.close()
            print("\nMIDI output closed.")

if __name__ == "__main__":
    main()
