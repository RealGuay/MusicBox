using MusicBox.Services.Interfaces.MusicSheetModels;
using System;

namespace MusicBox.Services.Interfaces
{
    public interface IBeatMaker : IDisposable
    {
        event EventHandler<BarReachedEventArgs> BarReached;

        event EventHandler<BeatReachedEventArgs> BeatReached;

        event EventHandler<SubBeatReachedEventArgs> SubBeatReached;

        event EventHandler<TickReachedEventArgs> TickReached;

        void SetParams(TimeSignature signature, int tempo, TickResolution tickResolution);

        void SetTempo(int newTempo);

        void Start();

        void Stop();

        void RewindToZero();
    }
}