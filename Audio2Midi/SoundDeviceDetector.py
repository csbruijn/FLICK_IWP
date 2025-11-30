import mido

print("Available MIDI output ports:")
for name in mido.get_output_names():
    print(" -", name)
