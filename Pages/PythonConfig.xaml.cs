using ExperimentInterface.Backend;
using ExperimentInterface.Backend.Interactions.Python;
using ExperimentInterface.CustomControls;
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
    /// Interaction logic for PythonConfig.xaml
    /// </summary>
    public partial class PythonConfig : Page
    {
        Session session;
        string? currentInterpreter = String.Empty;

        public PythonConfig()
        {
            InitializeComponent();
            session = Session.Instance;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            currentInterpreter = PythonHelper.FindPythonPath();

            if (currentInterpreter != null)
            {
                PythonNotConfiguredButFound.Visibility = Visibility.Visible;
                Path.Text = currentInterpreter;
            }
            else
            {
                PythonNotConfiguredOrFound.Visibility = Visibility.Visible;
            }
        }

        private void OnNavigationButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationButton? pressedButton = e.OriginalSource as NavigationButton;

            if (pressedButton != null)
            {
                NavigationService.GoBack();
            }
        }

        private void OnAcceptButtonClick(object sender, RoutedEventArgs e)
        {
            session.Interpreter = currentInterpreter;

            PythonNotConfiguredOrFound.Visibility = Visibility.Collapsed;
            PythonNotConfiguredButFound.Visibility = Visibility.Collapsed;
            PythonConfigured.Visibility = Visibility.Visible;

            ConfiguredPath.Text = currentInterpreter;
        }

        private void OnBrowseButtonClick(object sender, RoutedEventArgs e)
        {
            string? promptedPath = PythonHelper.PromptForPythonPath();

            if (promptedPath != null)
            {
                currentInterpreter = promptedPath;
                session.Interpreter = promptedPath;

                PythonNotConfiguredOrFound.Visibility = Visibility.Collapsed;
                PythonNotConfiguredButFound.Visibility = Visibility.Collapsed;
                PythonConfigured.Visibility = Visibility.Visible;

                ConfiguredPath.Text = currentInterpreter;
            }
        }

        private void OnInstallButtonClick(object sender, RoutedEventArgs e)
        {
            InstallModules.Visibility = Visibility.Collapsed;
            BeingInstalled.Visibility = Visibility.Visible;

            PythonHelper.InstallDependencies();

            BeingInstalled.Visibility = Visibility.Collapsed;
            Installed.Visibility = Visibility.Visible;
        }
    }
}
