using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ExperimentInterface.Backend
{
    /// <summary>
    /// Class that handles reading from and writing to disk. Allows for loading tasks and saving results.
    /// </summary>
    internal class DataManager
    {
        private string data = "ExperimentInterface.Data.Tasks.csv";
        private string savePath;

        private bool writing = false;

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
        private List<Task> ReadTasks()
        {
            List<Task> tasks = new List<Task>();

            try
            {
                using (Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(data))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string? row;

                    while ((row = reader.ReadLine()) != null)
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
        private void WriteResults(List<TaskResult> results)
        {
            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(savePath, "Results.csv"), true))
            {
                writing = true;

                foreach (TaskResult result in results)
                {
                    outputFile.WriteLine(ResultToRow(result));
                }

                writing = false;
            }
        }

        /// <summary>
        /// Converts a given <c>TaskResult</c> into a CSV row. To support the data analysis, saves booleans as integers
        /// </summary>
        /// <param name="result"></param>
        /// <returns>A comma separated <c>string</c> describing a result</returns>
        private string ResultToRow(TaskResult result)
        {
            return $"{result.participantID}, {result.interactionMethod}, {result.task.id}, {result.secondsTaken}, {(result.feedbackTask ? 1 : 0)}, {(result.givenFeedback ? 1 : 0)}";
        }

        #endregion

        #region Handles

        internal void Save(List<TaskResult> results)
        {
            Trace.WriteLine("Now Saving!");
            WriteResults(results);
        }

        internal List<Task> Load() { return ReadTasks(); }

        internal bool IsWriting() { return writing; }

        internal int GetLastParticipantID()
        {
            int participantID = 0;

            try
            {
                string lastLine = File.ReadLines(System.IO.Path.Combine(savePath, "Results.csv")).Last();

                if (lastLine != null)
                {
                    participantID = (Int32.TryParse(lastLine.Split(",")[0], out int parsedID)) ? parsedID : 0;
                }
            }
            catch (FileNotFoundException e)
            {
                Trace.TraceError(e.Message);
            }

            return participantID;
        }

        #endregion
    }
}
