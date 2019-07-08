using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Settings for our noise filters.
/// Changes how we interpret the noise.
/// </summary>
[System.Serializable]
public class NoiseSettings
{
    //FilterType enum
    public enum FilterType { Simple, Rigid };

    //The current filter type.
    public FilterType filterType;

    //Only show these settings if it is filtertype enum 0, which is simple.
    [ConditionalHide("filterType", 0)]
    public SimpleNoiseSettings simpleNoiseSettings;

    [ConditionalHide("filterType", 1)]
    public RigidNoiseSettings rigidNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        /// <summary>
        /// How many layers of noise we want to use on the surface. 
        /// We need multiple layers to simulate different things. One Top layer for landmasses, another for terrain on those masses, and so on.
        /// </summary>
        [Range(1, 8)]
        public int numLayers = 1;

        /// <summary>
        /// Strength of the noise
        /// </summary>
        public float strength = 1;

        /// <summary>
        /// Base roughness of the first noise layer.
        /// </summary>
        public float baseRoughness = 1;

        /// <summary>
        /// Roughness of the noise
        /// </summary>
        public float roughness = 2;

        /// <summary>
        /// How much of a reduction there is as we go down through the layers as they're applied to the surface.
        /// Default value of .5f means that as we go down through each layer, each layer affects it by half in comparison to the one before it.
        /// </summary>
        public float persistence = .5f;

        public float minValue;

        /// <summary>
        /// Center of the noise. Allows us to move it so different parts of the noise will be used.
        /// </summary>
        public Vector3 center;
    }

    [System.Serializable]
    public class RigidNoiseSettings: SimpleNoiseSettings
    {
        /// <summary>
        /// Weight multiplier. Weight multiplier causes low regions to have less detail.
        /// </summary>
        public float weightMultiplier = .8f;
    }

 


}
