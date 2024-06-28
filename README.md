# ISBEP: Experiment Interface
Interface used to evaluate the usability of different interaction methods (buttons, voice, and gestures) in an experiment on human-computer interaction. This tool was created as part of Vanderlande's ISBEP challenge at the Eindhoven University of Technology. Written for Windows machines in C# / WPF and some Python.

## Setup
After building the solution, the tool _should_ work as is, **except for the gesture controls**. These are implemented using an embedded Python script which uses the MediaPipe module. This currently requires an external Python 3.x interpreter with the modules specified in `dependencies.txt` installed. The tool will attempt to auto-detect a Python install and will otherwise prompt for one. It can optionally attempt to install the required modules from within the tool.

Voice recognition requires a microphone and currently uses Window's Speech dll.

Gesture detection requires a webcam and may take a few seconds to initialize. As it is performed its own process, do not forget to manually end it (by default, `escape` is bound to this).

## Inactive and Archived
Following the end of the ISBEP project, this repository has been archived.
