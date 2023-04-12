using MusicBox.Services.Interfaces;
using MusicBox.Services.Interfaces.MusicSheetModels;
using MusicBox.Services.MidiInterfaces;
using System;
using System.Collections.Generic;

namespace MusicBox.Services
{
    public class MidiPlayer : IMidiPlayer
    {
        private readonly List<MidiNoteToPlay> _midiNotes;
        private readonly IBeatMaker _beatMaker;
        private readonly IMidiOutputDevice _midiOutputDevice;
        private int _noteIndex;
        private int _lastNoteIndex;
        private const int default_Tempo = 60;

        public event Action<int, int> PlayingProgress;

        public event Action<bool> PlayingState;

        public MidiPlayer(IBeatMaker beatMaker, IMidiOutputDevice midiOutputDevice)
        {
            _midiNotes = new List<MidiNoteToPlay>();
            _beatMaker = beatMaker;
            _beatMaker.SetParams(TimeSignature.TS_4_4, default_Tempo, TickResolution.Normal);
            _beatMaker.TickReached += _beatMaker_TickBeatReached;
            _midiOutputDevice = midiOutputDevice;
            _noteIndex = 0;
        }

        private void _beatMaker_TickBeatReached(object sender, TickReachedEventArgs e)
        {
            int midiChannel = 0;
            MidiNoteToPlay note = _midiNotes[_noteIndex];
            while (note.TickTimeToPlay <= e.TicksFromZero)
            {
                _midiOutputDevice.PlayNote(midiChannel, note.MidiKey, note.Volume);
                PlayingProgress?.Invoke(e.TicksFromZero, note.MidiKey);  // todo use real params
                if (_noteIndex == _lastNoteIndex)
                {
                    StopAndRewind();
                    break;
                }
                else
                {
                    note = _midiNotes[++_noteIndex];
                }
            }
        }

        private void StopAndRewind()
        {
            _beatMaker.Stop();
            PlayingState?.Invoke(false);
            _beatMaker.RewindToZero();
        }

        public void PlaySheet(SheetInformation sheetInfo, int tempo)
        {
            _midiNotes.Clear();
            ConvertToMidiNotes(sheetInfo);
            PlayMidiNotes(tempo);
        }

        private void ConvertToMidiNotes(SheetInformation sheetInfo)
        {
            int positionOffset = 0;
            foreach (Segment segment in sheetInfo.Segments)
            {
                positionOffset += ConvertToMidiNotes(segment, positionOffset);
            }
        }

        public void PlaySegment(Segment segment, int tempo)
        {
            _midiNotes.Clear();
            ConvertToMidiNotes(segment, 0);
            PlayMidiNotes(tempo);
        }

        private void PlayMidiNotes(int tempo)
        {
            _midiNotes.Sort((a, b) => a.TickTimeToPlay.CompareTo(b.TickTimeToPlay));
            if (_midiNotes.Count > 0)
            {
                _noteIndex = 0;
                _lastNoteIndex = _midiNotes.Count - 1;
            }

            _beatMaker.Stop();
            _beatMaker.SetTempo(tempo);
            _beatMaker.Start();
            PlayingState?.Invoke(true);
        }

        private int ConvertToMidiNotes(Segment segment, int positionOffset)
        {
            int tickPerBar = (int)TickResolution.Normal * segment.TimeSignature.BeatsPerBar;

            foreach (Bar bar in segment.Bars)
            {
                foreach (SheetNote note in bar.SheetNotes)
                {
                    int absoluteOnPosition = positionOffset + (bar.PlayOrder * tickPerBar) + note.PositionInBar;
                    int absoluteOffPosition = absoluteOnPosition + note.Duration - 1; // make sure to Release before replaying the same note

                    MidiNoteToPlay midiNoteOn = new MidiNoteToPlay { TickTimeToPlay = absoluteOnPosition, MidiKey = note.Key, Volume = 100 };
                    _midiNotes.Add(midiNoteOn);

                    MidiNoteToPlay midiNoteOff = new MidiNoteToPlay { TickTimeToPlay = absoluteOffPosition, MidiKey = note.Key, Volume = 0 };
                    _midiNotes.Add(midiNoteOff);
                }
            }
            return segment.Bars.Count * tickPerBar; // position of the beginning of the next segment
        }

        public void Pause()
        {
            _beatMaker.Stop();
            PlayingState?.Invoke(false);
        }

        public void RewindToZero()
        {
            _beatMaker.Stop();
            _beatMaker.RewindToZero();
        }
    }
}