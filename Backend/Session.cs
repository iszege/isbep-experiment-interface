using System;

namespace ExperimentInterface.Backend
{
    /// <summary>
    /// Represents a session of running the application. Responsible for initializing and indexing helper classes to be
    /// accesed by the frontend.
    /// </summary>
    internal class Session
    {
        private static readonly Lazy<Session> lazyInstance = new Lazy<Session>(() => new Session());

        internal static Session Instance
        {
            get
            {
                return lazyInstance.Value;
            }
        }

        internal TaskManager taskManager = new();
        internal ExperimentData experimentData = new();

        internal string? Interpreter { get; set; }
    }
}
