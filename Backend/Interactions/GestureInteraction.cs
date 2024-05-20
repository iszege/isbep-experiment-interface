using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private readonly SynchronizationContext _syncContext;

        internal static event Action<string>? OnGestureDetected;

        private bool started = false;
        private bool interrupted = false;

        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = "C:\\Users\\20192685\\AppData\\Local\\Programs\\Python\\Python311\\python.exe",
            Arguments = "C:\\Users\\20192685\\Desktop\\GestureDetection\\gesture_detection_thumbs_embedded.py",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        internal GestureInteraction(SynchronizationContext syncContext)
        {
            _syncContext = syncContext;
            StartMonitoring();
        }

        internal void StartMonitoring()
        {
            // If already monitoring, do not start another process
            if (started) return;

            // Start a new async Task for the monitoring behavior
            started = true;
            new System.Threading.Tasks.Task(Monitor).Start();
        }

        internal void StopMonitoring()
        {
            // Sets interrupted to true, killing the process in Monitor()
            interrupted = true;
            started = false;
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

                        Trace.WriteLine(result);

                        _syncContext.Post(e => OnGestureDetected?.Invoke(result), null);

                        if (result == "Thumbs Up")
                        {
                            process.Kill();
                        }
                        else if (result == "Thumbs Down")
                        {
                            
                        }
                        
                        if (this.interrupted) process.Kill();
                    }
                }

                string error = process.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(error))
                {
                    Trace.WriteLine("Error: " + error);
                }
            }
        }

        private void Communicate(string result)
        {
            try
            {
                _syncContext.Post(e => OnGestureDetected?.Invoke(result), null);
            }

            catch (InvalidOperationException exc)
            {
                MessageBox.Show(exc.ToString());
            }

            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

    }
}
