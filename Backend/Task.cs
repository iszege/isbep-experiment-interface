using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentInterface.Backend
{
    /// <summary>
    /// Represents a task to be completed by the participant.
    /// Includes an ID, product name and possibly the number of such products present
    /// </summary>
    internal class Task
    {
        internal int ID;
        internal string Name;
        internal int Amount;

        public Task()
        {

        }
    }
}
