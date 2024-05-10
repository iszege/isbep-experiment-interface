namespace ExperimentInterface.Backend
{
    /// <summary>
    /// Represents a session of running the application. Responsible for initializing and indexing helper classes to be
    /// accesed by the frontend.
    /// </summary>
    internal class Session
    {
        internal TaskManager taskManager = new();
        internal ParticipantData participantData = new();
    }
}
