namespace ExperimentInterface.Backend
{
    /// <summary>
    /// Represents a task to be completed by the participant.
    /// Includes an ID, product name and possibly the number of such products present
    /// </summary>
    internal class Task
    {
        internal int id;
        internal string name;
        internal int amount;

        public Task(int id, string name, int amount)
        {
            this.id = id;
            this.name = name;
            this.amount = amount;
        }
    }
}
