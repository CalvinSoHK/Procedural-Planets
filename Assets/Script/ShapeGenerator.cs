using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will calculate where points should be on the sphere
/// </summary>
public class ShapeGenerator
{
    //The current settings
    ShapeSettings settings;

    //NoiseFilter
    INoiseFilter[] noiseFilters;

    //MinMax class that will keep track of our lowest and highest point on the mesh.
    public MinMax elevationMinMax;

    /// <summary>
    /// Changed from constructor to be consistent with ColorGenerator, which changed to be more memory efficient.
    /// </summary>
    /// <param name="settings"></param>
    public void UpdateSettings(ShapeSettings settings)
    {
        this.settings = settings;
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        for(int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }

        elevationMinMax = new MinMax();
    }

    /// <summary>
    /// Takes in a point on the sphere and returns where that point should be in accordance to shape settings.
    /// </summary>
    /// <param name="pointOnUnitSphere"></param>
    /// <returns></returns>
    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        //Get an elevation value
        float firstLayerValue = 0;
        float elevation = 0;

        //We want to save the first layer value so we can use it as a mask if need be.
        //However, since we are already computing it here, might as well not compute it in the loop below.
        //Therefore, we have to set elevation to firstLayerValue if the first layer is enabled at all.
        if(noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for(int i = 1; i < noiseFilters.Length; i++)
        {
            if (settings.noiseLayers[i].enabled)
            {
                //If we are set to use first layer as mask, this will return 0, otherwise it will be 1.
                //We then multiply the value with the mask so it will only be used if it is 1.
                float mask = (settings.noiseLayers[i].useFirstLayerMask) ? firstLayerValue : 1;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }       
        }

        //Calculate elevation for given point.
        elevation = settings.planetRadius * (1 + elevation);

        //Add value to elevationMinMax. This just processes the value and saves it as either min or max if it fits the criteria.
        elevationMinMax.AddValue(elevation);

        //Take the point on the unit sphere, and elevate it to the target location
        return pointOnUnitSphere * elevation;
    }
}
