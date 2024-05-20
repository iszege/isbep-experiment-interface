using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentInterface.Backend
{
    /// <summary>
    /// Class concerned with generating and tracking tasks, presenting them to the frontend and retrieving corresponding results
    /// </summary>
    internal class TaskManager
    {
        private DataManager dataManager = new DataManager();
        private Random random = new Random();
        
        private List<Task> tasks = new List<Task>();
        private List<TaskResult> results = new List<TaskResult>();

        internal int feedbackTasks { get; set; }
        internal int feedbackInterval { get; set; }
        internal int remainingTasks { get; private set; }
        
        internal TaskManager()
        {
            GetRequiredTasks();
            LoadTasks();
        }

        #region Setup methods

        internal void Reset()
        {
            GetRequiredTasks();
        }

        private void GetRequiredTasks()
        {
            feedbackTasks = 5;
            feedbackInterval = 2;
            remainingTasks = (1 + feedbackInterval) * feedbackTasks;
        }

        #endregion

        #region Loading and Saving

        private void LoadTasks()
        {
            tasks = dataManager.Load();
            tasks = tasks.OrderBy(_ => random.Next()).ToList();
        }

        internal void SaveResults()
        {
            dataManager.Save(results);
            results = new List<TaskResult>();
        }

        #endregion

        #region Generating and Presenting Tasks

        /// <summary>
        /// Returns the next task to be presented, or null if the interaction method is completed and no tasks remain.
        /// </summary>
        /// <returns>The <c>Task</c> to be completed, or <c>null</c> if no tasks remain</returns>
        internal Task? GetNextTask()
        {
            if (remainingTasks <= 0) return null;
            if (tasks.Count == 0) LoadTasks();

            Task task = tasks.First();
            task.instructions = GetTaskInstructions(task);
            tasks.RemoveAt(0);
            remainingTasks--;

            return task;
        }

        /// <summary>
        /// Generates a set of instructions for the provided <c>Task</c>, 
        /// currently contains a number and a basket to place that number of items into.
        /// </summary>
        /// <param name="task"></param>
        /// <returns>A <c>string[]</c> containing two instructions</returns>
        private string[] GetTaskInstructions(Task task)
        {
            int amount = random.Next(1, task.amount + 1);
            
            string[] sides = new string[] { "Left", "Right" };
            string side = sides[random.Next(sides.Length)];

            return new string[] { amount.ToString(), side };
        }

        /// <summary>
        /// Determines whether the next task to be completed should include a feedback task.
        /// </summary>
        /// <returns><c>True</c> if feedback should be completed, <c>False</c> otherwise</returns>
        internal bool GetNextTaskType()
        {
            return (remainingTasks % (feedbackInterval + 1) == 0 ) ? true : false;
        }

        #endregion

        #region Retrieving and Storing Results

        internal void AddResult(ExperimentData participant, Task task, double secondsTaken, bool feedbackTask, bool givenFeedback)
        {
            TaskResult result = new TaskResult(participant.ID, participant.Interaction, task, secondsTaken, feedbackTask, givenFeedback);
            results.Add(result);
        }

        #endregion
    }
}
