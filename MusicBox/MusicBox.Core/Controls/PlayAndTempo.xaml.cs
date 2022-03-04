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



        public int Pat_Tempo
        {
            get { return (int)GetValue(Pat_TempoProperty); }
            set { SetValue(Pat_TempoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pat_Tempo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Pat_TempoProperty =
            DependencyProperty.Register("Pat_Tempo", typeof(int), typeof(PlayAndTempo), new PropertyMetadata(0));


        public ICommand MyStartCommand
        {
            get { return (ICommand)GetValue(MyStartCommandProperty); }
            set { SetValue(MyStartCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyStartCommand.This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyStartCommandProperty =
            DependencyProperty.Register("MyStartCommand", typeof(ICommand), typeof(PlayAndTempo), new PropertyMetadata(null));



        public ICommand MyStopCommand
        {
            get { return (ICommand)GetValue(MyStopCommandProperty); }
            set { SetValue(MyStopCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyStopCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyStopCommandProperty =
            DependencyProperty.Register("MyStopCommand", typeof(ICommand), typeof(PlayAndTempo), new PropertyMetadata(null));




        public bool IsPlaying
        {
            get { return (bool)GetValue(IsPlayingProperty); }
            set { SetValue(IsPlayingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPlaying.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPlayingProperty =
            DependencyProperty.Register("IsPlaying", typeof(bool), typeof(PlayAndTempo), new PropertyMetadata(false));




        public ICommand RToZCommand
        {
            get { return (ICommand)GetValue(RToZCommandProperty); }
            set { SetValue(RToZCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RToZCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RToZCommandProperty =
            DependencyProperty.Register("RToZCommand", typeof(ICommand), typeof(PlayAndTempo), new PropertyMetadata(null));




    }
}