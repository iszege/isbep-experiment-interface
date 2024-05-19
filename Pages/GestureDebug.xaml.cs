using ExperimentInterface.Backend.Interactions;
using System;
using System.Diagnostics;
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

        private async void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            await WaitForDraw();
            gestureInteraction = new GestureInteraction();
            gestureInteraction.Start();
            GestureInteraction.FeedbackGiven += GestureInteraction_FeedbackGiven;
            //ReadFeedback();
        }

        private void GestureInteraction_FeedbackGiven(string obj)
        {
            Trace.WriteLine(obj);
        }

        /// <summary>
        /// Implements an asynchronous delay before starting the python process to ensure all UI is drawn.
        /// </summary>
        private async Task WaitForDraw()
        {
            await Task.Delay(10);
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
