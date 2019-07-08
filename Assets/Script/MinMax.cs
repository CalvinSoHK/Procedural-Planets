using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMax
{
    //Set min and max to public get but private set
    public float Min { get; private set; }
    public float Max { get; private set; }

    /// <summary>
    /// Constructor for MinMax. Sets Min to the max float, and Max to the min float.
    /// </summary>
    public MinMax()
    {
        Min = float.MaxValue;
        Max = float.MinValue;
    }

    /// <summary>
    ///  Function that accounts for a value. Takes no action if the new value is neither a new min or max.
    /// </summary>
    /// <param name="v"></param>
    public void AddValue(float v)
    {
        if(v > Max)
        {
            Max = v;
        }
        if(v < Min)
        {
            Min = v;
        }
    }
}
