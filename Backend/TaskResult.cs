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
        internal int interactionMethod;
        internal int taskID;
        internal int secondsTaken;
        internal bool feedbackTask;
        internal bool givenFeedback;

        internal TaskResult(int participantID, int interactionMethod, int taskID, int secondsTaken, bool feedbackTask, bool givenFeedback)
        {
            this.participantID = participantID;
            this.interactionMethod = interactionMethod;
            this.taskID = taskID;
            this.secondsTaken = secondsTaken;
            this.feedbackTask = feedbackTask;
            this.givenFeedback = givenFeedback;
        }
    }
}
