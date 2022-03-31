using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MusicBox.Core.Controls
{
    /// <summary>
    /// Interaction logic for PlayAndTempo.xaml
    /// </summary>
    public partial class PlayAndTempo : UserControl
    {
        public PlayAndTempo()
        {
            InitializeComponent();
        }

        public int PlayTempo
        {
            get { return (int)GetValue(PlayTempoProperty); }
            set { SetValue(PlayTempoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayTempo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayTempoProperty =
            DependencyProperty.Register("PlayTempo", typeof(int), typeof(PlayAndTempo), new PropertyMetadata(0));

        public ICommand PlayStartCommand
        {
            get { return (ICommand)GetValue(PlayStartCommandProperty); }
            set { SetValue(PlayStartCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayStartCommand.This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayStartCommandProperty =
            DependencyProperty.Register("PlayStartCommand", typeof(ICommand), typeof(PlayAndTempo), new PropertyMetadata(null));

        public ICommand PlayStopCommand
        {
            get { return (ICommand)GetValue(PlayStopCommandProperty); }
            set { SetValue(PlayStopCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayStopCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayStopCommandProperty =
            DependencyProperty.Register("PlayStopCommand", typeof(ICommand), typeof(PlayAndTempo), new PropertyMetadata(null));

        public bool PlayIsPlaying
        {
            get { return (bool)GetValue(PlayIsPlayingProperty); }
            set { SetValue(PlayIsPlayingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayIsPlaying.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayIsPlayingProperty =
            DependencyProperty.Register("PlayIsPlaying", typeof(bool), typeof(PlayAndTempo), new PropertyMetadata(false));

        public ICommand PlayRewindCommand
        {
            get { return (ICommand)GetValue(PlayRewindCommandProperty); }
            set { SetValue(PlayRewindCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayRewindCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayRewindCommandProperty =
            DependencyProperty.Register("PlayRewindCommand", typeof(ICommand), typeof(PlayAndTempo), new PropertyMetadata(null));
    }
}