using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExperimentInterface.Backend.Interactions
{
    /// <summary>
    /// Class that implements the gesture control logic used in the gesture interactions.
    /// Uses an external python file using the MediaPipe module for gesture detection.
    /// </summary>
    internal class GestureInteraction
    {
        internal string? pyResult;
        internal bool isPickable;

        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = "C:\\Users\\20192685\\AppData\\Local\\Programs\\Python\\Python311\\python.exe",
            Arguments = "C:\\Users\\20192685\\Desktop\\GestureDetection\\gesture_detection_thumbs.py",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        internal GestureInteraction()
        {
            using (Process process = Process.Start(processStartInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    while (!reader.EndOfStream)
                    {
                        string result = reader.ReadLine();
                        Trace.WriteLine(result);
                        pyResult = result;
                        //if (result == "Thumbs Up")
                        //{
                        //    Console.WriteLine("Detected Thumbs Up gesture.");
                        //    // Respond to thumbs up gesture
                        //    // e.g., call a function, update UI, etc.
                        //}
                        //else if (result == "Thumbs Down")
                        //{
                        //    Console.WriteLine("Detected Thumbs Down gesture.");
                        //    // Respond to thumbs down gesture
                        //    // e.g., call a function, update UI, etc.
                        //}
                    }
                }

                string error = process.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(error))
                {
                    Trace.WriteLine("Error: " + error);
                }
            }
        }
    }
}
