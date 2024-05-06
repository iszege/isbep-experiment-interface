using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using static System.Formats.Asn1.AsnWriter;

namespace ExperimentInterface.Backend
{
    internal class DataManager
    {
        string docPath;

        internal DataManager()
        {
            docPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\ISBEP\\ISBEP Experiment Interface";
            Directory.CreateDirectory(docPath);
        }

        #region Task Reading and Conversion

        internal List<Task> ReadTasks()
        {
            List<Task> tasks = new List<Task>();

            try
            {
                using (StreamReader inputFile = new StreamReader(System.IO.Path.Combine(docPath, "Items.csv"))) //TODO: Make relative?
                {
                    string row;

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

        private Task RowToTask(string row)
        {
            return null;
        }

        #endregion

        #region Result Writing and Conversion

        internal void WriteResults(List<TaskResult> results)
        {
            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(docPath, "Results.csv"), true))
            {
                foreach (TaskResult result in results)
                {
                    outputFile.WriteLine(ResultToRow(result));
                }
            }
        }

        private string ResultToRow(TaskResult result)
        {
            return null;
        }

        #endregion

        #region Handles
        #endregion
    }
}
