using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{
    /// <summary>
    /// Radius of planet
    /// </summary>
    public float planetRadius = 1;

    /// <summary>
    /// All the noise layers we want to use on our planet
    /// </summary>
    public NoiseLayer[] noiseLayers;

    /// <summary>
    /// Noise layer being used by our shape generator.
    /// </summary>
    [System.Serializable]
    public class NoiseLayer
    {
        /// <summary>
        /// Allow us to toggle this layer from being used.
        /// </summary>
        public bool enabled = true;

        /// <summary>
        /// Allows us to use the first layer as a mask for effects. This will allow us to only have mountains on continents for example.
        /// </summary>
        public bool useFirstLayerMask;

        /// <summary>
        /// The noise settings this layer uses.
        /// </summary>
        public NoiseSettings noiseSettings;
    }
}
