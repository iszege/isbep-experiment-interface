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
        internal static event Action<string>? FeedbackGiven;
        internal bool isPickable;

        ProcessStartInfo processStartInfo;

        internal GestureInteraction()
        {
            processStartInfo = new ProcessStartInfo
            {
                FileName = "C:\\Users\\20192685\\AppData\\Local\\Programs\\Python\\Python311\\python.exe",
                Arguments = "C:\\Users\\20192685\\Desktop\\GestureDetection\\gesture_detection_thumbs_embedded.py",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
        }

        internal void Start()
        {
            using (Process process = Process.Start(processStartInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    while (!reader.EndOfStream)
                    {
                        string result = reader.ReadLine();

                        if (result == "Thumbs Up")
                        {
                            Trace.WriteLine("Detected Thumbs Up gesture.");
                            FeedbackGiven?.Invoke("Yes");
                        }
                        else if (result == "Thumbs Down")
                        {
                            Trace.WriteLine("Detected Thumbs Down gesture.");
                        }
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
