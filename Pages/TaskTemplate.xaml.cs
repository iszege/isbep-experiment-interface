using ExperimentInterface.CustomControls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
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

        Backend.Task? currentTask;

        public TaskTemplate()
        {
            InitializeComponent();
            NextTask();
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
            FeedbackControls.Visibility = (feedback) ? Visibility.Visible : Visibility.Collapsed;
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

            // TODO navigate to a new exit page or simply back to the landing page
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

        #endregion
    }
}
