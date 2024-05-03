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
    /// Interaction logic for Landing.xaml
    /// </summary>
    public partial class Landing : Page
    {
        Window mainWindow = Application.Current.MainWindow;

        public Landing()
        {
            InitializeComponent();
        }

        private void OnNavigationCardClick(object sender, RoutedEventArgs e)
        {
            NavigationCard? ClickedButton = e.OriginalSource as NavigationCard;
            Trace.WriteLine(ClickedButton);

            if (ClickedButton != null)
            {
                NavigationService.Navigate(ClickedButton.NavigationUri);
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
        }

        private void OnResizeButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow? window = mainWindow as MainWindow;
            if (window != null) window.ToggleFullscreen();

            UpdateResizeIcon();
        }

        private void UpdateResizeIcon()
        {
            switch (mainWindow.WindowState)
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
    }
}
