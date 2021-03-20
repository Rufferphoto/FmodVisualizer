using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
/// <summary>
/// Please note this script does not require DataManager.
/// This script is a basic example of using a line renderer that creates and updates a waveform by using the gathered data from FMOD.
/// You can use this to ensure the spectrum data is being correctly gathered. If your FMOD project is correctly configured then the line will update.
/// </summary>

namespace FMODVisualizer
{
    public class WaveLineRenderer : MonoBehaviour
    {

        // FMOD
        FMOD.DSP fft;
        const int windowSize = 1024;
        const float width = 100.0f;
        const float height = 0.5f;
        LineRenderer lineRenderer;

        void Start()
        {
            // Get lineRenderer
            lineRenderer = gameObject.GetComponent<LineRenderer>();
            // Setting the amount of positions on the line to the amount of floats from the spectrum.
            lineRenderer.positionCount = windowSize;
            // Setting size of the line.
            lineRenderer.startWidth = .1f;
            lineRenderer.endWidth = .1f;
            // Setting up Blackman spectrum.
            FMODUnity.RuntimeManager.CoreSystem.createDSPByType(FMOD.DSP_TYPE.FFT, out fft);
            fft.setParameterInt((int)FMOD.DSP_FFT.WINDOWTYPE, (int)FMOD.DSP_FFT_WINDOW.BLACKMAN);
            fft.setParameterInt((int)FMOD.DSP_FFT.WINDOWSIZE, windowSize * 2);

            FMOD.ChannelGroup channelGroup;
            FMODUnity.RuntimeManager.CoreSystem.getMasterChannelGroup(out channelGroup);
            channelGroup.addDSP(FMOD.CHANNELCONTROL_DSP_INDEX.HEAD, fft);
        }


        void Update()
        {
            FMODVisuals();
        }

        void FMODVisuals()
        {
            // FMOD
            IntPtr unmanagedData;
            uint length;
            // Getting the data from fmod and assigning it to unmanagedData with a length.
            fft.getParameterData((int)FMOD.DSP_FFT.SPECTRUMDATA, out unmanagedData, out length);
            FMOD.DSP_PARAMETER_FFT fftData = (FMOD.DSP_PARAMETER_FFT)Marshal.PtrToStructure(unmanagedData, typeof(FMOD.DSP_PARAMETER_FFT));
            var spectrum = fftData.spectrum;
            // Make sure there are channels currently.
            if (fftData.numchannels > 0)
            {
                var pos = Vector3.zero;
                pos.x = width * -0.5f;
                // Go through the list within the size of the window of data gathered. i.e. 512. each i represents each position in the line.
                for (int i = 0; i < windowSize; i++)
                {
                    pos.x += (width / windowSize);
                    // Assign the data from spectrum to the level of this point in the line.
                    float level = lin2dB(spectrum[0][i]);
                    pos.y = (80 + level) * height;
                    lineRenderer.SetPosition(i, pos);
                }
            }
        }

        float lin2dB(float linear)
        {
            return Mathf.Clamp(Mathf.Log10(linear) * 20.0f, -80.0f, 0.0f);
        }
    }
}
