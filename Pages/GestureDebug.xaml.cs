using ExperimentInterface.Backend.Interactions;
using System;
using System.Threading;
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
        SynchronizationContext? syncContext;

        public GestureDebug()
        {
            InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            // Get the SynchronizationContext to be able to post events from a different thread inside GestureInteraction
            syncContext = SynchronizationContext.Current ?? throw new ArgumentNullException(nameof(syncContext));

            // Initialize a new GestureInteraction using the SynchronizationContext
            gestureInteraction = new GestureInteraction(syncContext);

            // Subscribe to the event
            GestureInteraction.OnGestureDetected += GestureInteraction_OnGestureDetected;
        }

        private void OnPageUnloaded(object sender, RoutedEventArgs e)
        {
            // Unsubscribe from the event
            GestureInteraction.OnGestureDetected -= GestureInteraction_OnGestureDetected;
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void GestureInteraction_OnGestureDetected(string obj)
        {
            Output.Text = obj;
        }
    }
}
