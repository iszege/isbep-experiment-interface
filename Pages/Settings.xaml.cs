using ExperimentInterface.CustomControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ExperimentInterface.Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        MainWindow? mainWindow = Application.Current.MainWindow as MainWindow;

        public Settings()
        {
            InitializeComponent();
            if (mainWindow != null)
            {
                CurrentParticipantText.Text = $"The current participant is participant #{mainWindow.session.experimentData.ID}." +
                                              $"\n\n Set up a new participant profile ?";
            }
        }

        private void OnNavigationButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationButton? ClickedButton = e.OriginalSource as NavigationButton;

            if (ClickedButton == SaveButton)
            {
                if (mainWindow != null) mainWindow.session.experimentData.ID++;
                NavigationService.GoBack();
            }
            else
            {
                NavigationService.GoBack();
            }
        }

        private void OnNewParticipantClick(object sender, RoutedEventArgs e)
        {
            CurrentParticipantText.Visibility = Visibility.Collapsed;
            NewParticipantText.Visibility = Visibility.Visible;
            NewParticipant.Visibility = Visibility.Collapsed;
        }

        private void OnDebugClick(object sender, RoutedEventArgs e)
        {
            NavigationButton? ClickedButton = e.OriginalSource as NavigationButton;

            if (ClickedButton != null)
            {
                NavigationService.Navigate(ClickedButton.NavigationUri);
            }
        }
    }
}
