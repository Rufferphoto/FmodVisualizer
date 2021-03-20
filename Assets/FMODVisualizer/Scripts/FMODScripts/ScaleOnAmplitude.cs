using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script changes the scale of an object depending on the amplitude of the track playing.
/// </summary>

namespace FMODVisualizer
{
    public class ScaleOnAmplitude : MonoBehaviour
    {

        // Public
        public float startScale, maxScale;
        // Smooth out visuals using buffer.
        public bool useBuffer;
        // Set true if you wish the object to lerp through colours.
        public bool colourLerp;
        // Set the speed of the lerp.
        public float lerpSpeed;
        // Set colour for object to lerp through. Min = min audio intensity, Max = max audio intensity.
        public Color colourMin, colourMax;

        // Private
        Material _material;

        // Use this for initialization
        void Start()
        {
            _material = GetComponent<MeshRenderer>().materials[0];
        }
        
        // Update is called once per frame
        void Update()
        {
            ScaleUpdate();
        }

        private void ScaleUpdate()
        {
            if (useBuffer && DataManager.amplitudeBuffer > 0)
            {
                // Changing local scale depending on current FMOD amplitude.
                transform.localScale = new Vector3((DataManager.amplitudeBuffer * maxScale) + startScale,
                    (DataManager.amplitudeBuffer * maxScale) + startScale,
                    (DataManager.amplitudeBuffer * maxScale) + startScale);
                // Lerp through the colours
                if (colourLerp)
                {
                    _material.color = Color.Lerp(colourMin, colourMax, DataManager.amplitudeBuffer * lerpSpeed);
                }
            }
            if (!useBuffer && DataManager.amplitude > 0)
            {
                // Changing local scale depending on current FMOD amplitude.
                transform.localScale = new Vector3((DataManager.amplitude * maxScale) + startScale,
                    (DataManager.amplitude * maxScale) + startScale,
                    (DataManager.amplitude * maxScale) + startScale);
                // Lerp through the colours.
                if (colourLerp)
                {
                    _material.color = Color.Lerp(colourMin, colourMax, DataManager.amplitude * lerpSpeed);
                }
            }
        }
    }
}

