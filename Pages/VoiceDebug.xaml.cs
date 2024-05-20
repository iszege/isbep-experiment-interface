using ExperimentInterface.Backend.Interactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ExperimentInterface.Pages
{
    /// <summary>
    /// Interaction logic for VoiceDebug.xaml
    /// </summary>
    public partial class VoiceDebug : Page
    {
        VoiceInteraction? voiceInteraction;

        public VoiceDebug()
        {
            InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            voiceInteraction = new VoiceInteraction();
            VoiceInteraction.OnFeedbackGiven += VoiceInteraction_OnFeedbackGiven;
        }

        private void OnPageUnloaded(object sender, RoutedEventArgs e)
        {
            if (voiceInteraction != null) voiceInteraction.Dispose();
            VoiceInteraction.OnFeedbackGiven -= VoiceInteraction_OnFeedbackGiven;
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void VoiceInteraction_OnFeedbackGiven(bool pickable)
        {
            Output.Text = (pickable) ? "Yes" : "No";
        }
    }
}
