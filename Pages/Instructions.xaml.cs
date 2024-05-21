using ExperimentInterface.CustomControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ExperimentInterface.Pages
{
    /// <summary>
    /// Interaction logic for Instructions.xaml
    /// </summary>
    public partial class Instructions : Page
    {
        MainWindow? mainWindow = Application.Current.MainWindow as MainWindow;

        #region Instruction strings

        string buttonInstructions = "you can submit you choice by pressing the arrow keys on the keyboard (left for \"no\", right for \"yes\").";
        string voiceInstructions = "you can submit you choice by saying either \"yes\" or \"no\".";
        string gestureInstructions = "you can submit you choice by making a \"Thumbs Up\" or \"Thumbs Down\" gesture to the camera.";

        string currentInstructions = string.Empty;

        #endregion

        public Instructions()
        {
            InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            int interaction = 0;

            if (mainWindow != null)
            {
                interaction = mainWindow.session.experimentData.Interaction;
            }

            switch (interaction)
            {
                case 1:
                    currentInstructions = buttonInstructions;
                    break;
                
                case 2:
                    currentInstructions = voiceInstructions;
                    break;
                
                case 3:
                    currentInstructions = gestureInstructions;
                    break;

                default:
                    currentInstructions = buttonInstructions;
                    break;
            }

            GenerateInstructions();
        }

        private void GenerateInstructions()
        {
            Description.Text =
                "In this part of the experiment, you will be given 15 tasks to perform. The aim is to complete these tasks as quickly as possible. " +
                "\n\n" +
                "In each task, you will be asked to place one of the items in front of you in either the basket on the left or " +
                "in the basket on the right. " +
                "After you have moved the item, or if the item was already in the requested basket, " +
                "you can continue to the next task by pressing the spacebar. " +
                "\n\n" +
                "During certain tasks, you will be asked whether you think that a robot would also be able to grab that item. " +
                "For this part of the experiment, " + currentInstructions + 
                "\n\n" +
                "Your time starts as soon as you click on \"Start Task\".";
        }

        private void OnNavigationButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationButton? ClickedButton = e.OriginalSource as NavigationButton;

            if (ClickedButton != null)
            {
                NavigationService.Navigate(ClickedButton.NavigationUri);
            }
        }

    }
}
