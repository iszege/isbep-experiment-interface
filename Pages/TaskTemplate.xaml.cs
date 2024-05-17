using ExperimentInterface.Backend.Interactions;
using ExperimentInterface.CustomControls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ExperimentInterface.Pages
{
    /// <summary>
    /// Interaction logic for TaskTemplate.xaml
    /// </summary>
    public partial class TaskTemplate : Page
    {
        MainWindow? mainWindow = Application.Current.MainWindow as MainWindow;
        Stopwatch stopwatch = new Stopwatch();

        VoiceInteraction voiceInteraction = new VoiceInteraction();
        Backend.Task? currentTask;

        Grid? FeedbackControls;

        public TaskTemplate()
        {
            InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            NextTask();
            
            Application.Current.MainWindow.KeyDown += OnKeyDown;
            VoiceInteraction.FeedbackGiven += VoiceInteraction_FeedbackGiven;
        }
       
        private void OnPageUnloaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown -= OnKeyDown;
            VoiceInteraction.FeedbackGiven -= VoiceInteraction_FeedbackGiven;
        }

        #region Task Setup

        /// <summary>
        /// Loads and displays the next task to be completed and starts the data collection or signals when no tasks remain
        /// </summary>
        private void NextTask()
        {
            if (mainWindow != null)
            {
                // Load next task
                Backend.Task? NextTask = mainWindow.session.taskManager.GetNextTask();
                bool feedback = mainWindow.session.taskManager.GetNextTaskType();

                // Populate the page or exit in case no new task was loaded
                if (NextTask != null)
                {
                    PopulateFields(NextTask, feedback);
                    currentTask = NextTask;
                    stopwatch.Restart();
                }
                else
                {
                    Exit();
                }
            }
        }
        
        /// <summary>
        /// Populates the relevant fields on the task page with data from the loaded task
        /// </summary>
        private void PopulateFields(Backend.Task task, bool feedback)
        {
            // Set text and images
            ItemName.Text = task.name;
            //ItemPicture.Source = new BitmapImage(GetImageUri(task.id));
            TaskInstructions.Text = GetTaskInstructions(task.name, task.instructions, feedback);

            // Toggle required controls
            DefaultControls.Visibility = (feedback) ? Visibility.Collapsed : Visibility.Visible;
            ToggleFeedbackControls(feedback);
        }

        /// <summary>
        /// Enables the active feedback controls for this interaction type, disables all others.
        /// </summary>
        private void ToggleFeedbackControls(bool feedback)
        {
            if (mainWindow != null)
            {
                Grid[] controls = { FeedbackButtons, FeedbackVoice, FeedbackGestures };

                if (feedback)
                {
                    for (int i = 1; i <= controls.Length; i++)
                    {
                        if (i == mainWindow.session.experimentData.Interaction)
                            controls[i - 1].Visibility = Visibility.Visible;
                        else 
                            controls[i - 1].Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    foreach (Grid control in controls)
                    {
                        control.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a relative URI to the image corresponding to the provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A relative URI</returns>
        private Uri GetImageUri(int id)
        {
            return new Uri($"/Images/Tasks/{id}", UriKind.Relative);
        }

        /// <summary>
        /// Generates instructions for the current task based on its name and embedded instructions and on whether feedback is required
        /// </summary>
        /// <param name="name"></param>
        /// <param name="instructions"></param>
        /// <param name="feedback"></param>
        /// <returns>A <c>string</c> containing all instructions to be displayed</returns>
        private string GetTaskInstructions(string name, string[]? instructions, bool feedback)
        {
            // Ensure missing instructions are dealt with
            string amount = (instructions != null) ? instructions[0] : "1";
            string side = (instructions != null) ? instructions[1] : "left";

            // Determine if name should be plural
            int numAmount = (Int32.TryParse(amount, out int parsedAmount)) ? parsedAmount : 1;
            name = (numAmount > 1) ? $"{name}s" : name;

            // Generate and return instructions
            string taskInstruction = $"Place exactly {amount} {name} (as shown on screen) in the basket on the {side.ToLower()}.";
            string inputInstruction = (feedback)
                                      ? "Complete the feedback task described below to continue."
                                      : "Press the spacebar or click the button below to continue.";

            return taskInstruction + "\n\n" + inputInstruction;
        }

        /// <summary>
        /// Called when all tasks for this interaction are completed, closes the task page
        /// </summary>
        private void Exit()
        {
            if (mainWindow != null) mainWindow.session.taskManager.SaveResults();
            currentTask = null;

            // Dispose the SpeechRecognitionEngine when ending the voice recognition task
            if (mainWindow != null && mainWindow.session.experimentData.Interaction == 2) voiceInteraction.Dispose();

            // Navigate to the task completed page
            NavigationService.Navigate(new Uri("/Pages/Done.xaml", UriKind.Relative));
        }

        #endregion

        #region Data Collection

        private void SaveResult(bool containsFeedback, bool isPickable)
        {
            // Stop the timer to log the time taken for this task
            stopwatch.Stop();

            // For validation, ensure that isPickable is always false if no feedback was given
            isPickable = containsFeedback && isPickable;

            // Communicate result to the TaskManager
            if (mainWindow != null && currentTask != null)
            {
                mainWindow.session.taskManager.AddResult(mainWindow.session.experimentData,
                                                         currentTask, 
                                                         stopwatch.Elapsed.TotalSeconds, 
                                                         containsFeedback, 
                                                         isPickable);
            }

            // Load the next task
            NextTask();
        }

        #endregion

        #region Input Listeners

        private void OnNextTaskButtonClick(object sender, RoutedEventArgs e)
        {
            SaveResult(false, false);
        }

        private void OnFeedbackButtonClick(object sender, RoutedEventArgs e)
        {
            TaskButton? ClickedButton = e.OriginalSource as TaskButton;

            if (ClickedButton == RobotButton)
                SaveResult(true, true);
            else
                SaveResult(true, false);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (mainWindow == null) return;

            // If no feedback is needed, let spacebar load the next task
            if (e.Key == Key.Space && !mainWindow.session.taskManager.GetNextTaskType())
                SaveResult(false, false);

            // Input other than space is only relevant to feedback tasks in the button interaction method
            if (!mainWindow.session.taskManager.GetNextTaskType()) return;
            if (mainWindow.session.experimentData.Interaction != 1) return;

            // Send to robot button is expected on the left, send to human on the right
            if (e.Key == Key.Right)
                SaveResult(true, false);
            else if (e.Key == Key.Left)
                SaveResult(true, true);
        }

        private void VoiceInteraction_FeedbackGiven(bool obj)
        {
            Trace.WriteLine(obj);
        }

        #endregion
    }
}
