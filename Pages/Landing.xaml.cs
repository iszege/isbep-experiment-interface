using ExperimentInterface.Backend;
using ExperimentInterface.CustomControls;
using System;
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
        Session session;

        public Landing()
        {
            InitializeComponent();
            session = Session.Instance;
            UpdateUI();
        }

        private void OnNavigationCardClick(object sender, RoutedEventArgs e)
        {
            NavigationCard? ClickedButton = e.OriginalSource as NavigationCard;

            // Set the current interaction type if applicable
            if (ClickedButton == InteractionMethod1) session.experimentData.Interaction = 1;
            if (ClickedButton == InteractionMethod2) session.experimentData.Interaction = 2;
            if (ClickedButton == InteractionMethod3) session.experimentData.Interaction = 3;


            // In any case, navigate to the specified URI if a NavigationCard was indeed clicked, or the Python config screen if needed
            if (ClickedButton != null)
            {
                if (session.experimentData.Interaction == 3 && session.Interpreter == null) 
                    NavigationService.Navigate(new Uri("/Pages/PythonConfig.xaml", UriKind.Relative));
                else
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
            ParticipantID.Content = $"Participant #{session.experimentData.ID}";
        }
    }
}
