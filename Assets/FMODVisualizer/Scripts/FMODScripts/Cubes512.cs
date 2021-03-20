using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script spawns the ring of cubes around the track and then updates the scale depending on the music playing.
/// </summary>

namespace FMODVisualizer
{
    public class Cubes512 : MonoBehaviour
    {
        // Public
        public GameObject _sampleCubePrefab;
        public float maxScale;
        public int cubeAmount;
        public bool useBuffer;
        public float startScale, scaleMultiplier;

        // Private
        public GameObject[] _sampleCube = new GameObject[512]; // Create 512 cubes.

        void Start()
        {
            // Create 512 cubes.
            for (int i = 0; i < 512; i++)
            {
                GameObject _instanceSampleCube = (GameObject)Instantiate(_sampleCubePrefab);
                // Centralising prefab.
                _instanceSampleCube.transform.position = transform.position;
                _instanceSampleCube.transform.parent = transform;
                _instanceSampleCube.name = "SampleCube" + i;

                // Setting cubes to the correct position to form a circle.
                transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
                _instanceSampleCube.transform.position = Vector3.back * 100;
                _sampleCube[i] = _instanceSampleCube;
            }
        }

        void Update()
        {
            ScaleUpdate();
        }

        private void ScaleUpdate()
        {
            // Updating scales.
            // Buffer is less jagedy.
            if (useBuffer)
            {
                // Going through each cube.
                for (int i = 0; i < 512; i++)
                {
                    if (_sampleCube != null)
                    {
                        // Updating the scale.
                        // Uses data gathered from FMOD.
                        _sampleCube[i].transform.localScale = new Vector3(1, (DataManager.samplesBuffer[i] * scaleMultiplier) + startScale, transform.localScale.z);
                    }
                }
            }
            // jaggedy visuals
            if (!useBuffer)
            {
                for (int i = 0; i < 512; i++)
                {
                    if (_sampleCube != null)
                    {
                        _sampleCube[i].transform.localScale = new Vector3(transform.localScale.x, (DataManager.samples[i] * maxScale) + 2, 1);
                    }
                }
            }
        }
    }
}