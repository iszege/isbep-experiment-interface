using System;
using System.Diagnostics;
using System.Speech.Recognition;

namespace ExperimentInterface.Backend.Interactions
{
    /// <summary>
    /// Class that implements the voice control logic used in the voice interactions.
    /// Uses the SpeechRecognitionEngine Class
    /// </summary>
    internal class VoiceInteraction
    {
        SpeechRecognitionEngine recognizer;

        internal static event Action<bool>? FeedbackGiven;

        internal VoiceInteraction()
        {
            // Initialize the SpeechRecognitionEngine
            recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));

            // Create and load a grammar for a binary choice (could possibly expand this in the future)
            Choices options = new Choices(new string[] {"yes", "no"});
            GrammarBuilder grammarBuilder = new GrammarBuilder(options);

            recognizer.LoadGrammarAsync(new Grammar(grammarBuilder));

            // Add a handler for the speech recognized event.  
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(OnSpeechRecognized);

            // Configure input to the speech recognizer.  
            recognizer.SetInputToDefaultAudioDevice();

            // Start asynchronous, continuous speech recognition.  
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private static void OnSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text)
            {
                case "yes":;
                    FeedbackGiven?.Invoke(true);
                    break;

                case "no":
                    FeedbackGiven?.Invoke(false);
                    break;

                default:
                    break;
            }
        }

        internal void Dispose()
        {
            recognizer.Dispose();
        }
    }
}
