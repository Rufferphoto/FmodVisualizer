using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script changes the scale of an object depending on the frequency range set by the audio band of the track playing.
/// </summary>

namespace FMODVisualizer
{
    public class ScaleOnBand : MonoBehaviour
    {

        // Public
        public float startScale, maxScale;
        // Audio band to react to.
        public int _audioBand;
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
            if (useBuffer && DataManager.audioBandBuffer[_audioBand] > 0)
            {
                // Changing local scale depending on current audio band.
                transform.localScale = new Vector3((DataManager.audioBandBuffer[_audioBand] * maxScale) + startScale,
                (DataManager.audioBandBuffer[_audioBand] * maxScale) + startScale,
                (DataManager.audioBandBuffer[_audioBand] * maxScale) + startScale);
                // Lerp through the colours.
                if (colourLerp)
                {
                    _material.color = Color.Lerp(colourMin, colourMax, DataManager.audioBandBuffer[_audioBand] * lerpSpeed);
                }
            }
            if (!useBuffer && DataManager.audioBand[_audioBand] > 0)
            {
                // Changing local scale depending on current audio band.
                transform.localScale = new Vector3((DataManager.audioBand[_audioBand] * maxScale) + startScale,
                (DataManager.audioBand[_audioBand] * maxScale) + startScale,
                (DataManager.audioBand[_audioBand] * maxScale) + startScale);
                // Lerp through the colours.
                if (colourLerp)
                {
                    _material.color = Color.Lerp(colourMin, colourMax, DataManager.audioBand[_audioBand] * lerpSpeed);
                }
            }
        }

        
    }
}