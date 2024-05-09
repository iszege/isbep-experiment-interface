using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ExperimentInterface.Backend
{
    internal class DataManager
    {
        string dataURI = "../Data/Tasks.csv";
        string savePath;

        internal DataManager()
        {
            savePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\ISBEP\\ISBEP Experiment Interface";
            Directory.CreateDirectory(savePath);
        }

        #region Task Reading and Conversion

        /// <summary>
        /// Reads tasks from CSV into memory
        /// </summary>
        /// <returns>A list of <c>Task</c> objects</returns>
        internal List<Task> ReadTasks()
        {
            List<Task> tasks = new List<Task>();

            try
            {
                using (StreamReader inputFile = new StreamReader(System.IO.Path.GetFullPath(dataURI)))
                {
                    string? row;

                    while ((row = inputFile.ReadLine()) != null)
                    {
                        tasks.Add(RowToTask(row));
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Trace.TraceError(e.Message);
            }

            return tasks;
        }

        /// <summary>
        /// Converts CSV row into Task object. Expects input row to be formatted as <c>[ID, Name, Amount]</c>
        /// </summary>
        /// <param name="row"></param>
        /// <returns>A <c>Task</c> object representation of the CSV row</returns>
        private Task RowToTask(string row)
        {
            string[] columns = row.Split(',');

            int id = (Int32.TryParse(columns[0], out int parsedID)) ? parsedID : 0;
            int amount = (Int32.TryParse(columns[2], out int parsedAmount)) ? parsedAmount : 0;

            return new Task(id, columns[1], amount);
        }

        #endregion

        #region Result Writing and Conversion

        /// <summary>
        /// Writes a given list of <c>TaskResult</c>s into Results.csv
        /// </summary>
        /// <param name="results"></param>
        internal void WriteResults(List<TaskResult> results)
        {
            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(savePath, "Results.csv"), true))
            {
                foreach (TaskResult result in results)
                {
                    outputFile.WriteLine(ResultToRow(result));
                }
            }
        }

        /// <summary>
        /// Converts a given <c>TaskResult</c> into a CSV row. To support the data analysis, saves booleans as integers
        /// </summary>
        /// <param name="result"></param>
        /// <returns>A comma separated <c>string</c> describing a result</returns>
        private string ResultToRow(TaskResult result)
        {
            return $"{result.taskID}, {result.interactionMethod}, {result.taskID}, {result.secondsTaken}, {(result.feedbackTask ? 1 : 0)}, {(result.givenFeedback ? 1 : 0)}";
        }

        #endregion

        #region Handles
        #endregion
    }
}
