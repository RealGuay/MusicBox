using MusicBox.Services.MidiInterfaces;
using Sanford.Multimedia.Midi;
using System.Collections.Generic;

namespace MusicBox.Services.MidiSanford
{
    public class MidiOutputDevice : IMidiOutputDevice
    {
        private OutputDevice outDev1;

        public MidiOutputDevice(int deviceId = -1)
        {
            if (deviceId == -1)
            {
                deviceId = OutputDevice.DeviceCount - 1;
            }
            outDev1 = new OutputDevice(deviceId);
        }

        public void PlayNote(int channel, int note, int volume)
        {
            outDev1.Send(new ChannelMessage(ChannelCommand.NoteOn, channel, note, volume));
        }

        public void ReleaseNote(int channel, int note, int volume)
        {
            outDev1.Send(new ChannelMessage(ChannelCommand.NoteOff, channel, note, volume));
        }

        public void CloseDevice()
        {
            outDev1.Close();
        }

        public void Dispose()
        {
            outDev1.Dispose();
        }

        public static List<string> GetDeviceNames()
        {
            List<string> outMidiDevs = new List<string>();

            for (int i = 0; i < OutputDeviceBase.DeviceCount; i++)
            {
                var capabilities = OutputDeviceBase.GetDeviceCapabilities(i);
                outMidiDevs.Add(capabilities.name);
            }
            return outMidiDevs;
        }
    }
}