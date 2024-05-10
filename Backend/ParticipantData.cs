namespace ExperimentInterface.Backend
{
    /// <summary>
    /// Class responsible for storing and manipulating data on the current participant.
    /// Contains a participant ID and the current Interaction Method.
    /// </summary>
    internal class ParticipantData
    {
        internal int ID { get; set; }
        internal int Interaction { get; set; }

        internal ParticipantData()
        {
            ID = new DataManager().GetLastParticipantID() + 1;
        }
    }
}
