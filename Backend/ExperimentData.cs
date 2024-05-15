namespace ExperimentInterface.Backend
{
    /// <summary>
    /// Class responsible for storing and manipulating data on the current experiment stage.
    /// Contains a participant ID and the current Interaction Method.
    /// </summary>
    internal class ExperimentData
    {
        internal int ID { get; set; }
        internal int Interaction { get; set; }

        internal ExperimentData()
        {
            ID = new DataManager().GetLastParticipantID() + 1;
        }
    }
}
