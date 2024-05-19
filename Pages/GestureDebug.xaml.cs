using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ExperimentInterface.Pages
{
    /// <summary>
    /// Interaction logic for GestureDebug.xaml
    /// </summary>
    public partial class GestureDebug : Page
    {
        public GestureDebug()
        {
            InitializeComponent();
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
