using ExperimentInterface.Backend;
using ExperimentInterface.CustomControls;
using System;
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
        Session session;

        public Settings()
        {
            InitializeComponent();
            session = Session.Instance;
            CurrentParticipantText.Text = $"The current participant is participant #{session.experimentData.ID}. \n\n Set up a new participant profile ?";
        }

        private void OnNavigationButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationButton? ClickedButton = e.OriginalSource as NavigationButton;

            if (ClickedButton == SaveButton)
            {
                session.experimentData.ID++;
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
            NavigationButton? clickedButton = e.OriginalSource as NavigationButton;

            if (clickedButton != null)
            {
                if (clickedButton == GestureDebug && session.Interpreter == null)
                    NavigationService.Navigate(new Uri("/Pages/PythonConfig.xaml", UriKind.Relative));
                else
                    NavigationService.Navigate(clickedButton.NavigationUri);
            }
        }
    }
}
