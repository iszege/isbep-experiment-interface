using ExperimentInterface.CustomControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ExperimentInterface.Pages
{
    /// <summary>
    /// Interaction logic for Landing.xaml
    /// </summary>
    public partial class Landing : Page
    {
        Window window = Application.Current.MainWindow as Window;
        MainWindow? mainWindow = Application.Current.MainWindow as MainWindow;

        public Landing()
        {
            InitializeComponent();
            UpdateUI();
        }

        private void OnNavigationCardClick(object sender, RoutedEventArgs e)
        {
            NavigationCard? ClickedButton = e.OriginalSource as NavigationCard;

            // Set the current interaction type if applicable
            if (mainWindow != null)
            {
                if (ClickedButton == InteractionMethod1) mainWindow.session.experimentData.Interaction = 1;
                if (ClickedButton == InteractionMethod2) mainWindow.session.experimentData.Interaction = 2;
                if (ClickedButton == InteractionMethod3) mainWindow.session.experimentData.Interaction = 3;
            }

            // In any case, navigate to the specified URI if a NavigationCard was indeed clicked
            if (ClickedButton != null)
            {
                NavigationService.Navigate(ClickedButton.NavigationUri);
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            window.Close();
        }

        private void OnResizeButtonClick(object sender, RoutedEventArgs e)
        {
            if (mainWindow != null) mainWindow.ToggleFullscreen();

            UpdateResizeIcon();
        }

        private void UpdateResizeIcon()
        {
            switch (window.WindowState)
            {
                case WindowState.Normal:
                    MaximizeButton.Visibility = Visibility.Visible;
                    MinimizeButton.Visibility = Visibility.Collapsed;
                    break;

                case WindowState.Maximized:
                    MaximizeButton.Visibility = Visibility.Collapsed;
                    MinimizeButton.Visibility = Visibility.Visible;
                    break;

                default:
                    break;
            }
        }

        private void OnToolbarButtonClick(object sender, RoutedEventArgs e)
        {
            ToolbarButton? ClickedButton = e.OriginalSource as ToolbarButton;

            if (ClickedButton != null)
            {
                NavigationService.Navigate(ClickedButton.NavigationUri);
            }
        }

        /// <summary>
        /// If needed, updates UI elements upon reloading the page
        /// </summary>
        private void UpdateUI()
        {
            if (mainWindow == null) return;
            ParticipantID.Content = $"Participant #{mainWindow.session.experimentData.ID}";
        }
    }
}
