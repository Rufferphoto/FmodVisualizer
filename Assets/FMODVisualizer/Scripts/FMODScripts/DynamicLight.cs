using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script changes the intensity of a light depending on the amplitude of a specific audioBand.
/// </summary>

namespace FMODVisualizer
{
    [RequireComponent(typeof(Light))]
    public class DynamicLight : MonoBehaviour
    {
        // Public 
        public float minIntensity, maxIntensity;
        // Audio band to react to.
        public int _audioBand;
        // Set true if you wish the object to lerp through colours.
        public bool colourLerp;
        // Set the speed of the lerp.
        public float lerpSpeed;
        // Set colour for object to lerp through. Min = min audio intensity, Max = max audio intensity.
        public Color colourMin, colourMax;

        // Private
        // Create new light instance.
        private new Light light;

        // Use this for initialization
        void Start()
        {
            light = GetComponent<Light>();
        }

        void Update()
        {
            // Changing intensity.
            if (DataManager.audioBandBuffer[_audioBand] > 0)
            {
                LightAudioIntensity();
            }
        }

        void LightAudioIntensity()
        {         
            // Lerp light colour between colourMin and colourMax if true.
            if (colourLerp)
            {
                light.color = Color.Lerp(colourMin, colourMax, DataManager.audioBandBuffer[_audioBand] * lerpSpeed);
            }
            // Change light intensity depending on the audioBandBuffer intensity.
            light.intensity = (DataManager.audioBandBuffer[_audioBand] * (maxIntensity - minIntensity)) + minIntensity;       
        }
    }
}