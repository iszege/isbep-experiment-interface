using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ExperimentInterface.Backend.Interactions.Python
{
    /// <summary>
    /// Helper class concerned with locating Python scripts and interpreters
    /// </summary>
    internal static class PythonHelper
    {
        internal static string GetScriptPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gesture_detection_thumbs_embedded.py");
        }

        /// <summary>
        /// Attempts to find a Python interpreter by checking all default install paths for Python 3+ interpreters.
        /// Could probably achieve this more elegantly by checking the Registry or using Environment Variables.
        /// </summary>
        /// <returns><c>string path</c>: the path to the most recent interpreter found</returns>
        internal static string? FindPythonPath()
        {
            string localAppdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string[] defaultPaths = new[]
            {
                @"C:\Program Files\Python312\python.exe",
                @"C:\Program Files\Python311\python.exe",
                @"C:\Program Files\Python310\python.exe",
                @"C:\Program Files\Python39\python.exe",
                @"C:\Program Files\Python38\python.exe",
                @"C:\Program Files\Python37\python.exe",
                @"C:\Program Files\Python36\python.exe",
                @"C:\Program Files\Python35\python.exe",
                @"C:\Program Files\Python34\python.exe",
                @"C:\Program Files\Python33\python.exe",
                @"C:\Program Files\Python32\python.exe",
                @"C:\Program Files\Python31\python.exe",
                @"C:\Program Files\Python30\python.exe",
                localAppdata + @"\Programs\Python\Python312\python.exe",
                localAppdata + @"\Programs\Python\Python311\python.exe",
                localAppdata + @"\Programs\Python\Python310\python.exe",
                localAppdata + @"\Programs\Python\Python39\python.exe",
                localAppdata + @"\Programs\Python\Python38\python.exe",
                localAppdata + @"\Programs\Python\Python37\python.exe",
                localAppdata + @"\Programs\Python\Python36\python.exe",
                localAppdata + @"\Programs\Python\Python35\python.exe",
                localAppdata + @"\Programs\Python\Python34\python.exe",
                localAppdata + @"\Programs\Python\Python33\python.exe",
                localAppdata + @"\Programs\Python\Python32\python.exe",
                localAppdata + @"\Programs\Python\Python31\python.exe",
                localAppdata + @"\Programs\Python\Python30\python.exe"
            };

            foreach (string path in defaultPaths)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }

            return null;
        }

        internal static string? PromptForPythonPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Python Executable|python.exe",
                Title = "Select Python Interpreter"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            else
            {
                MessageBox.Show("Python interpreter not selected. The operation cannot proceed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        internal static void InstallDependencies()
        {
            Session session = Session.Instance;
            if (session.Interpreter == null) return;

            string dependencies = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dependencies.txt");
            if (!File.Exists(dependencies)) return;

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = session.Interpreter,
                Arguments = $"-m pip install -r {dependencies}",
                UseShellExecute = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                CreateNoWindow = false
            };

            using (Process process = Process.Start(processStartInfo))
            {
                //using (StreamReader reader = process.StandardOutput)
                //{
                //    string result = reader.ReadToEnd();
                //    Trace.WriteLine(result);
                //}

                //using (StreamReader reader = process.StandardError)
                //{
                //    string error = reader.ReadToEnd();
                //    Trace.WriteLine(error);
                //}
            }
        }
    }
}
