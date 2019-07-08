using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for noise filters. All scripts that implement INoiseFilter must have evaluate.
/// </summary>
public interface INoiseFilter
{
    float Evaluate(Vector3 point);
}
