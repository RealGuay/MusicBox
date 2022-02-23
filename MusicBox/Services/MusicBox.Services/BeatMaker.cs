using MusicBox.Services.Interfaces;
using MusicBox.Services.MidiInterfaces;
using MusicBox.Services.MidiSanford;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBox.Services
{
    public class BeatMaker : IBeatMaker
    {
        private IMidiTimer _midiTimer;

        public BeatMaker()
        {
            _midiTimer = new MidiTimer();

            _midiTimer.TickDetected += TimerTickDetected;
            _midiTimer.Period = 5000;
            _midiTimer.Start();
           
        }

        public void Dispose()
        {
            _midiTimer?.Dispose();
        }

        private void TimerTickDetected(object sender, TickEventArg e)
        {
           
        
        }
    }
}
