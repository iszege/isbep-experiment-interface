using ExperimentInterface.Backend.Interactions;
using ExperimentInterface.CustomControls;
using System;
using System.Diagnostics;
using System.Threading;
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

        VoiceInteraction? voiceInteraction;
        GestureInteraction? gestureInteraction;

        Backend.Task? currentTask;

        int currentInteractionType = 1;

        Grid? FeedbackControls;

        public TaskTemplate()
        {
            InitializeComponent();
        }

        #region Page Initialization

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            // Determine current interaction type
            if (mainWindow != null)
                currentInteractionType = mainWindow.session.experimentData.Interaction;

            // Get the current SynchronizationContext for async threads of the GestureInteraction
            SynchronizationContext syncContext = SynchronizationContext.Current ?? throw new ArgumentNullException(nameof(syncContext));

            // Initialize interaction helpers if needed
            if (currentInteractionType == 2) voiceInteraction = new VoiceInteraction();
            if (currentInteractionType == 3) gestureInteraction = new GestureInteraction(syncContext);

            // Subscribe to feedback events
            Application.Current.MainWindow.KeyDown += OnKeyDown;
            VoiceInteraction.OnFeedbackGiven += VoiceInteraction_OnFeedbackGiven;
            GestureInteraction.OnFeedbackGiven += GestureInteraction_OnFeedbackGiven;

            // Reset the amount of tasks remaining
            if (mainWindow != null) mainWindow.session.taskManager.Reset();

            // Load the first task
            NextTask();
        }

        private void OnPageUnloaded(object sender, RoutedEventArgs e)
        {
            // Unsubscribe from all feedback events
            Application.Current.MainWindow.KeyDown -= OnKeyDown;
            VoiceInteraction.OnFeedbackGiven -= VoiceInteraction_OnFeedbackGiven;
            GestureInteraction.OnFeedbackGiven -= GestureInteraction_OnFeedbackGiven;
        }

        #endregion

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
            // Set text
            ItemName.Text = task.name;
            SetTaskInstructions(task.name, task.instructions, feedback);

            // Set image
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = GetImageUri(task.id);
            bitmapImage.EndInit();
            ItemPicture.Source = bitmapImage;

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
            return new Uri($"/Images/Tasks/{id}.png", UriKind.Relative);
        }

        /// <summary>
        /// Generates instructions for the current task based on its name and embedded instructions and on whether feedback is required
        /// </summary>
        /// <param name="name"></param>
        /// <param name="instructions"></param>
        /// <param name="feedback"></param>
        private void SetTaskInstructions(string name, string[]? instructions, bool feedback)
        {
            // Ensure missing instructions are dealt with
            string side = (instructions != null) ? instructions[0] : "left";

            // Generate and fill instructions
            InstructionItem.Text = name.ToLower();
            InstructionSide.Text = side.ToLower();

            InstructionContinue.Text = (feedback)
                                      ? "Complete the feedback task described below to continue."
                                      : "Press the spacebar or click the button below to continue.";
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

        /// <summary>
        /// Event listener that registers key presses, used to communicate task completion and feedback.
        /// Implements logic for button controls interaction method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Event listener that registers speech feedback as either true ("yes") or false ("no").
        /// Implements logic for voice control interaction method.
        /// </summary>
        /// <param name="pickable"></param>
        private void VoiceInteraction_OnFeedbackGiven(bool pickable)
        {
            if (mainWindow == null) return;
            if (mainWindow.session.experimentData.Interaction != 2) return;
            if (!mainWindow.session.taskManager.GetNextTaskType()) return;

            //Trace.WriteLine($"Registered {((pickable) ? "yes" : "no")}, submitting feedback!");

            SaveResult(true, pickable);
        }

        /// <summary>
        /// Evebt listener that registers gesture feedback as either true ("yes") or false ("no").
        /// Implements logic for gesture control interaction method.
        /// </summary>
        /// <param name="pickable"></param>
        private void GestureInteraction_OnFeedbackGiven(bool pickable)
        {
            if (mainWindow == null) return;
            if (mainWindow.session.experimentData.Interaction != 3) return;
            if (!mainWindow.session.taskManager.GetNextTaskType()) return;

            //Trace.WriteLine($"Registered {((pickable) ? "yes" : "no")}, submitting feedback!");

            SaveResult(true, pickable);
        }

        #endregion
    }
}
