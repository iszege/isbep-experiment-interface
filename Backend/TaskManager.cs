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
        
        internal TaskManager()
        {
            LoadTasks();
        }

        #region Loading and Saving
        
        private void LoadTasks()
        {
            tasks = dataManager.Load();
            tasks = tasks.OrderBy(_ => random.Next()).ToList();
        }

        internal void SaveResults()
        {

        }

        #endregion

        #region Generating and Presenting Tasks

        internal Task GetNextTask()
        {
            if (tasks.Count == 0) LoadTasks();

            Task task = tasks.First();
            task.instructions = GetTaskInstructions(task);
            tasks.RemoveAt(0);

            return task;
        }

        private string[] GetTaskInstructions(Task task)
        {
            int amount = random.Next(1, task.amount + 1);
            
            string[] sides = new string[] { "Left", "Right" };
            string side = sides[random.Next(sides.Length)];

            return new string[] { amount.ToString(), side };
        }

        #endregion

        #region Retrieving and Storing Results

        internal void AddResult(ParticipantData participant, Task task, int secondsTaken, bool feedbackTask, bool givenFeedback)
        {
            TaskResult result = new TaskResult(participant.ID, participant.Interaction, task, secondsTaken, feedbackTask, givenFeedback);
            results.Add(result);
        }

        #endregion
    }
}
