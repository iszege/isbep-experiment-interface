# ISBEP: Experiment Interface
Interface used to evaluate the usability of different interaction methods (buttons, voice, and gestures) in an experiment on human-computer interaction. This tool was created as part of Vanderlande's ISBEP challenge at the Eindhoven University of Technology. Written in C# / WPF and some Python.

## Important for use
After building the solution, the tool _should_ out of the box, **except for the gesture controls**. These are implemented using an embedded Python script which uses the MediaPipe module. This currently requires an external Python 3.x interpreter with the modules specified in `dependencies.txt` installed. The tool will attempt to auto-detect a Python install and otherwise prompt for one. It can optionally attempt to install the required modules from within the tool.

## Archived
Following the end of the ISBEP project, this repository has been archived.