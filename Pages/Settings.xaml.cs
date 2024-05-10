using ExperimentInterface.CustomControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        private void OnNavigationButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationButton? ClickedButton = e.OriginalSource as NavigationButton;

            if (ClickedButton == SaveButton)
            {
                if (mainWindow != null) mainWindow.session.participantData.ID++;
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
    }
}
