using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script is an example of using an audioBand value to increase the .y vector of a Cube to simulate a graphic Equaliser.
/// </summary>

namespace FMODVisualizer
{
    public class GraphicEQ : MonoBehaviour
    {
        // Public
        public float _startScale, _scaleMultiplier;
        // Audio band to react to.
        public int _audioBand;
        // Smooth out visuals using buffer.
        public bool _useBuffer;

        void Update()
        {
            HeightScaleUpdate();
        }

        private void HeightScaleUpdate()
        {
            if (_useBuffer && DataManager.audioBandBuffer[_audioBand] > 0)
            {
                // Changing local scale depending on current audio band.
                transform.localScale = new Vector3(transform.localScale.x, (DataManager.audioBandBuffer[_audioBand] * _scaleMultiplier) + _startScale, transform.localScale.z);
            }
            if (!_useBuffer && DataManager.audioBand[_audioBand] > 0)
            {
                // Changing local scale depending on current audio band.
                transform.localScale = new Vector3(transform.localScale.x, (DataManager.audioBand[_audioBand] * _scaleMultiplier) + _startScale, transform.localScale.z);
            }
        }
    }
}