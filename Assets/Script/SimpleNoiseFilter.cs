using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    NoiseSettings.SimpleNoiseSettings settings;

    /// <summary>
    /// Noise script. Found from tutorial but not written by Sebastian. See script for specifics.
    /// Is not random noise, it's simplex noise.
    /// </summary>
    Noise noise = new Noise();

    /// <summary>
    /// Constructor. Assigns settings.
    /// </summary>
    /// <param name="settings"></param>
    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        this.settings = settings;
    }

    /// <summary>
    /// Evaluates a given noise point and returns a value we want for our generator.
    /// Roughness will make the terrain rougher by making us sample points further apart than just next to each other.
    /// Adding the center to it changes what center reference we are using to sample noise.
    /// Strenght is used to multiply the overall value to increase the intensity of the noise.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public float Evaluate(Vector3 point)
    {
        //For now we want to just move the data point from -1 to 1 to 0 to 1.
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;

        for(int i = 0; i < settings.numLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + settings.center);
            noiseValue += (v + 1) * 0.5f * amplitude;

            //By default roughness is greater than one. When that happens then the frequency of the noise increases as we go deeper.
            frequency *= settings.roughness;

            //By default persistence is less than one (but greater than zero), and so the amplitude is reduced every layer.
            amplitude *= settings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
        return noiseValue * settings.strength;
    }
}
