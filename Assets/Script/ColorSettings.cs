using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColorSettings : ScriptableObject
{
    //Gradient to use along the elevations
    public Gradient gradient;

    //Mat for planet rendering.
    public Material planetMaterial;
}
