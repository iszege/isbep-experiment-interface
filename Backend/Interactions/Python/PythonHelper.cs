using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        internal static string PromptForPythonPath()
        {
            return string.Empty;
        }
    }
}
