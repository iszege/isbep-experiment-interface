using ExperimentInterface.Backend.Interactions;
using System;
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
            ReadFeedback();
            // Output.Text = new GestureInteraction().pyResult;
            // Output.Text = new GestureInteraction().isPickable;
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ReadFeedback()
        {
            Output.Text = new GestureInteraction().pyResult;
        }
    }
}
