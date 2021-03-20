using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
/// <summary>
/// This script handles the functionality of gathering amplitudes and db data from FMOD audio system.
/// </summary>

namespace FMODVisualizer
{
    public class DataManager : MonoBehaviour
    {
        // FMOD
        FMOD.DSP fft;
        const int windowSize = 512;
        // Public Samples, FreqBands and Buffers
        public static float[] samples = new float[512];
        public static float[] samplesBuffer = new float[512];
        // Everytime the frequency band is higher than the band buffer the band buffer becomes the frequency band.
        // If frequency band is lower than the band buffer, then the band buffer will decrease by a set speed.
        public static float[] freqBand = new float[8];
        public static float[] bandbuffer = new float[8];
        // Declares 8 audio frequency bands, see CreateFreqBands() at the bottom of this script to see the frequency range for each band.
        public static float[] audioBand = new float[8];
        public static float[] audioBandBuffer = new float[8];
        public float _audioProfile;

        // Private
        float[] _samplesBufferDecrease = new float[512];
        float[] _bufferDecrease = new float[8];
        public float[] _freqBandHighest = new float[8];
        private IntPtr unmanagedData;
        private uint length;
        private float[][] spectrum;

        // Amplitude variables
        // Public 
        public float bufferDecreaseSpeed;
        public static float amplitude, amplitudeBuffer;
        // Private
        float _amplitudeHighest;

        void Start()
        {
            _amplitudeHighest = 1;
            Mathf.Clamp(bufferDecreaseSpeed, 0, 2); 
            // Setting up FFT.
            FMODUnity.RuntimeManager.CoreSystem.createDSPByType(FMOD.DSP_TYPE.FFT, out fft);
            fft.setParameterInt((int)FMOD.DSP_FFT.WINDOWTYPE, (int)FMOD.DSP_FFT_WINDOW.HANNING);
            fft.setParameterInt((int)FMOD.DSP_FFT.WINDOWSIZE, windowSize * 2);
            FMOD.ChannelGroup channelGroup;
            FMODUnity.RuntimeManager.CoreSystem.getMasterChannelGroup(out channelGroup);
            channelGroup.addDSP(FMOD.CHANNELCONTROL_DSP_INDEX.TAIL, fft);
            // Setting up audioProfile.
            AudioProfile(_audioProfile);

        }

        void Update()
        {
            GetSpectrum();
            SampleBuffer();
            CreateFreqBands();
            BandBuffer();
            CreateAudioBands();
            GetAmplitude();
        }

        void AudioProfile(float audioProfile)
        {
            for (int i = 0; i < 8; i++)
            {
                _freqBandHighest[i] = audioProfile;
            }
        }

        // Getting spectrum data function from the FMOD Channels.
        void GetSpectrum()
        {
            // Getting the data from fmod and assigning it to unmanagedData with a length.
            fft.getParameterData((int)FMOD.DSP_FFT.SPECTRUMDATA, out unmanagedData, out length);
            FMOD.DSP_PARAMETER_FFT fftData = (FMOD.DSP_PARAMETER_FFT)Marshal.PtrToStructure(unmanagedData, typeof(FMOD.DSP_PARAMETER_FFT));
            spectrum = fftData.spectrum;

            // Make sure there are channels currently.
            if (fftData.numchannels > 0)
            {
                // Go through the list within the size of the window of data gathered. i.e. 512
                for (int i = 0; i < windowSize; i++)
                {

                    // Assign the data from spectrum to the array of floats.
                    samples[i] = spectrum[0][i];
                }
            }
        }

        void SampleBuffer()
        {
            for (int i = 0; i < 512; i++)
            {
                // If samples is bigger than the buffer, bring the buffer up.
                if (samples[i] > samplesBuffer[i])
                {
                    samplesBuffer[i] = samples[i];
                    _samplesBufferDecrease[i] = 0.00005f;
                }
                // If samples is smaller than the buffer, reduce by the buffer decrease.
                if (samples[i] < samplesBuffer[i])
                {
                    samplesBuffer[i] -= _samplesBufferDecrease[i];
                    _samplesBufferDecrease[i] *= bufferDecreaseSpeed;
                }
            }
        }

        void GetAmplitude()
        {

            float _currentAmplitude = 0;
            float _currentAmplitudeBuffer = 0;
            for (int i = 0; i < 8; i++)
            {

                // Current amplitude is sum of all audio bands together.
                _currentAmplitude += audioBand[i];
                _currentAmplitudeBuffer += audioBandBuffer[i];
            }
            // Capping the current amplitude to the highest amplitude allowed.
            if (_currentAmplitude > _amplitudeHighest)
            {
                _amplitudeHighest = _currentAmplitude;
            }

            // Resetting values.
            amplitude = Mathf.Clamp01(_currentAmplitude / _amplitudeHighest);
            amplitudeBuffer = Mathf.Clamp01(_currentAmplitudeBuffer / _amplitudeHighest);
        }

        void CreateAudioBands()
        {
            for (int i = 0; i < 8; i++)
            {
                if (freqBand[i] > _freqBandHighest[i])
                {
                    // Store highest frequency value.
                    _freqBandHighest[i] = freqBand[i];
                }
                // Getting the audioBand and audioBand buffer from FMOD.
                audioBand[i] = (freqBand[i] / _freqBandHighest[i]);
                audioBandBuffer[i] = (bandbuffer[i] / _freqBandHighest[i]);
            }
        }

        void BandBuffer()
        {
            for (int g = 0; g < 8; ++g)
            {
                // If samples is bigger than the buffer, bring the buffer up.
                if (freqBand[g] > bandbuffer[g])
                {
                    bandbuffer[g] = freqBand[g];
                    _bufferDecrease[g] = 0.0005f;
                }
                // If samples is smaller than the buffer, reduce by the buffer decrease.
                if (freqBand[g] < bandbuffer[g])
                {
                    bandbuffer[g] -= _bufferDecrease[g];
                    _bufferDecrease[g] *= bufferDecreaseSpeed;
                }
            }
        }

        void CreateFreqBands()
        {
            /*
             * 22050 / 512 = 43hz per sample 
             * 
             * 20-60hz 
               60-250hz
               250-500hz
               500-2000hz
               2000-4000hz
               4000-6000hz
               6000-20000hz   

             * How many of these samples do we need to divide over these frequency bands
             * 
             * 0-2 = 86hz
             * 1-4 = 172hz      range 87 - 258
             * 2-8 = 344hz      range 259 - 60
             * 3-16 = 688hz     range 603 - 1290
             * 4-32 = 1376hz    range 1291 -2666
             * 5-64 = 2752hz    range 2667 - 5418
             * 6-128 = 5504hz   range 5419 - 10922
             * 7-256 = 11008hz  range 10923 - 21930
             * total 510 frequencys
             */
            int count = 0;
            // example if count = 0, 2 to the power of 0 = 1, 1 * 2 = 2. So would be in the first frequency band. 0-2 = 86hz
            for (int i = 0; i < 8; i++)
            {
                float average = 0;
                int sampleCount = (int)Mathf.Pow(2, i) * 2;

                // Including the 510-512 samples
                if (i == 7)
                {
                    sampleCount += 2;
                }
                for (int j = 0; j < sampleCount; j++)
                {
                    average += samples[count] * (count + 1);
                    count++;
                }
                average /= count;
                freqBand[i] = average;
            }
        }


    }
}
