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
        Window window = Application.Current.MainWindow as Window;
        MainWindow? mainWindow = Application.Current.MainWindow as MainWindow;

        public Landing()
        {
            InitializeComponent();
        }

        private void OnNavigationCardClick(object sender, RoutedEventArgs e)
        {
            NavigationCard? ClickedButton = e.OriginalSource as NavigationCard;

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
    }
}
