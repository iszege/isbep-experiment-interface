using ExperimentInterface.Backend.Interactions;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
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
        GestureInteraction? gestureInteraction;

        public GestureDebug()
        {
            InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            SynchronizationContext syncContext = SynchronizationContext.Current ?? throw new ArgumentNullException(nameof(syncContext));
            gestureInteraction = new GestureInteraction(syncContext);

            GestureInteraction.OnGestureDetected += GestureInteraction_OnGestureDetected;
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void OnDebugClick(object sender, RoutedEventArgs e)
        {
            gestureInteraction?.StopMonitoring();
        }

        private void GestureInteraction_OnGestureDetected(string obj)
        {
            Output.Text = obj;
        }
    }
}
