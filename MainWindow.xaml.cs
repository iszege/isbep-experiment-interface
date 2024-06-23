using System.Windows;

namespace ExperimentInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowClip.Rect = new Rect(0, 0, Width, Height);
        }

        public void ToggleFullscreen()
        {
            this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            WindowClip.Rect = new Rect(0, 0, Width, Height);
        }
    }
}
