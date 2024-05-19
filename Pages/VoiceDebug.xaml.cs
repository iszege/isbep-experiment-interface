using System;
using System.Collections.Generic;
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
    /// Interaction logic for VoiceDebug.xaml
    /// </summary>
    public partial class VoiceDebug : Page
    {
        public VoiceDebug()
        {
            InitializeComponent();
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
