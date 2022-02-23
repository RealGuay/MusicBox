using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBox.Services.MidiInterfaces
{
    public interface IMidiTimer : IDisposable
    {
        void Start();
        void Stop();
        int Period { get; set; }
        event EventHandler<TickEventArgs> TickDetected;
    }
}
