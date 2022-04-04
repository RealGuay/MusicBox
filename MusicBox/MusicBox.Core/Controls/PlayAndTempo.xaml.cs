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

        public int Tempo
        {
            get { return (int)GetValue(TempoProperty); }
            set { SetValue(TempoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Tempo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TempoProperty =
            DependencyProperty.Register("Tempo", typeof(int), typeof(PlayAndTempo), new PropertyMetadata(60));

        public ICommand PlayCommand
        {
            get { return (ICommand)GetValue(PlayCommandProperty); }
            set { SetValue(PlayCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayCommand.This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayCommandProperty =
            DependencyProperty.Register("PlayCommand", typeof(ICommand), typeof(PlayAndTempo), new PropertyMetadata(null));

        public ICommand PauseCommand
        {
            get { return (ICommand)GetValue(PauseCommandProperty); }
            set { SetValue(PauseCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PauseCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PauseCommandProperty =
            DependencyProperty.Register("PauseCommand", typeof(ICommand), typeof(PlayAndTempo), new PropertyMetadata(null));

        public bool IsPlaying
        {
            get { return (bool)GetValue(IsPlayingProperty); }
            set { SetValue(IsPlayingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPlaying.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPlayingProperty =
            DependencyProperty.Register("IsPlaying", typeof(bool), typeof(PlayAndTempo), new PropertyMetadata(false));

        public ICommand RewindCommand
        {
            get { return (ICommand)GetValue(RewindCommandProperty); }
            set { SetValue(RewindCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RewindCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RewindCommandProperty =
            DependencyProperty.Register("RewindCommand", typeof(ICommand), typeof(PlayAndTempo), new PropertyMetadata(null));
    }
}