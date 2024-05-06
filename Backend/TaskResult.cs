using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentInterface.Backend
{
    /// <summary>
    /// Represents the result after a task has been completed by the participant.
    /// Includes: Participant ID, Interaction Method, Time taken, Feedback task (bool), Given feedback (bool), and corresponding Task ID
    /// </summary>
    internal class TaskResult
    {
        internal int participantID;
        internal int interactionMehod;
        internal TimeSpan timeTaken;
        internal bool feedbackTask;
        internal bool givenFeedback;
        internal int taskID;

        internal TaskResult()
        {

        }
    }
}
