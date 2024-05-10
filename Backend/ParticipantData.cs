using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentInterface.Backend
{
    /// <summary>
    /// Represents a participant. Contains a participant ID and the current Interaction Method.
    /// </summary>
    internal class ParticipantData
    {
        internal int ID { get; set; }
        internal int Interaction { get; set; }
    }
}
