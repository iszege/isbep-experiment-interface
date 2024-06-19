using ExperimentInterface.Backend.Interactions.Python;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ExperimentInterface.Backend.Interactions
{
    /// <summary>
    /// Class that implements the gesture control logic used in the gesture interactions.
    /// Uses an external python file using the MediaPipe module for gesture detection.
    /// </summary>
    internal class GestureInteraction
    {
        private readonly SynchronizationContext _syncContext;

        internal static event Action<string>? OnGestureDetected;
        internal static event Action<bool>? OnFeedbackGiven;

        private bool started = false;

        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = PythonHelper.FindPythonPath() ?? PythonHelper.PromptForPythonPath(),
            Arguments = PythonHelper.GetScriptPath(),
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        internal GestureInteraction(SynchronizationContext syncContext)
        {
            // Load the SynchronizationContext
            _syncContext = syncContext;

            // Start the interaction
            StartMonitoring();

            PythonHelper.FindPythonPath();
        }

        internal void StartMonitoring()
        {
            // If already monitoring, do not start another process
            if (started) return;

            // Start a new async Task for the monitoring behavior
            started = true;
            new System.Threading.Tasks.Task(Monitor).Start();
        }

        private void Monitor()
        {
            using (Process process = Process.Start(processStartInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    while (!reader.EndOfStream)
                    {
                        string result = reader.ReadLine();

                        //Trace.WriteLine(result);

                        _syncContext.Post(e => OnGestureDetected?.Invoke(result), null);

                        if (result == "Thumbs Up")
                        {
                            _syncContext.Post(e => OnFeedbackGiven?.Invoke(true), null);
                        }
                        else if (result == "Thumbs Down")
                        {
                            _syncContext.Post(e => OnFeedbackGiven?.Invoke(false), null);
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
