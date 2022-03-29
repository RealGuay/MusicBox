namespace MusicBox.Services.MidiInterfaces
{
    public interface IMidiOutputDevice
    {
        void CloseDevice();

        void Dispose();

        void PlayNote(int channel, int note, int volume);

        void ReleaseNote(int channel, int note, int volume);
    }
}