using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    ColorSettings settings;

    //Texture that is made from our shadergraph
    Texture2D texture;

    //Resolution of texture
    const int textureResolution = 50;

    /// <summary>
    /// Changed from a constructor. This way we don't make multiple textures all the time.
    /// Just update the old file.
    /// </summary>
    /// <param name="settings"></param>
    public void UpdateSettings(ColorSettings settings)
    {
        this.settings = settings;
        if(texture == null)
        {
            texture = new Texture2D(textureResolution, 1);
        }
    }

    /// <summary>
    /// Updates the color based on elevation.
    /// </summary>
    /// <param name="elevationMinMax"></param>
    public void UpdateElevation(MinMax elevationMinMax)
    {
        //Set the vector elevationMinMax in our material to these values.
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[textureResolution];

        for(int i = 0; i < textureResolution; i++)
        {
            //Need the f to divide by float and we get decimals. Unity colors are from 0 to 1, where 1 is 255.
            colors[i] = settings.gradient.Evaluate(i / (textureResolution - 1f));
        }
        texture.SetPixels(colors);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}
