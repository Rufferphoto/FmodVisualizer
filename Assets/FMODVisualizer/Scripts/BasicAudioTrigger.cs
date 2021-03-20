using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script is a basic FMOD audio event trigger and should only be used as a test if FMOD is correctly configured with visualizations.
/// </summary>

namespace FMODVisualizer
{
    public class BasicAudioTrigger : MonoBehaviour
    {
        // FMOD
        [FMODUnity.EventRef]
        public string musicEvent;
        FMOD.Studio.EventInstance music;

        // Use this for initialization
        void Start()
        {
            music = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
            music.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(GetComponent<Transform>()));
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Play()
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(music, GetComponent<Transform>(), GetComponent<Rigidbody>());
            // Play the sound.
            music.start();
        }

        public void Stop()
        {
            music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        public void OnDestroy()
        {
            music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
}