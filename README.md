# FmodVisualizer
FMOD Visualizer is a unique tool that enables the use of raw audio data from your FMOD project to create dynamic audio visuals for your product.

Installing FMOD with your Unity Project
Please note this asset requires FMOD to be correctly added to your Unity Project already.
Please follow FMOD’s documentation if you are unsure of how to Integrate FMOD into your Unity 
project.
https://www.fmod.com/resources/documentation-unity?version=2.0&page=user-guide.html
https://www.fmod.com/download

Installation
Installing FMOD Visualiser will create the following folder structure:
FMODVisualiser:
Demo: A Simple demo scene
Documentation: FMOD Visualiser documentation
Scripts: FMOD Visualiser source code

Configuring FMOD
1. Once the package has been run and imported. The current FMOD project that you have 
configured in Unity, needs to be set to Stereo.
2. This is done within FMOD Studio, go to Edit > Preferences > Build > Project Platforms > [ Build 
target ] i.e. Desktop > Surround Speaker Mode > Stereo. See screenshots below.3
FMOD Visualiser by Dan Gregg (Ruffer) Version 1.0.0 – Last Updated September 2019

Using the demo Scene
1. Open Scenes/Demo.unity to load up an example of what FMODVisualiser can be used for.
2. Next select a music event from your FMOD Project for an example. Go into the hierarchy 
and select FMOD/AudioPeer and select a FMOD Music Event in the inspector window. See 
screenshots below.
3. Hit play on the editor window to start the scene and play again on the game view’s UI to 
start playing your selected FMOD Event. See screenshot on next page.
