using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The planet script generates the six TerrainFace objects that will then generate their own meshes.
/// NOTE: In Sebastian's video, he argues using the six face method (a cube that is "blown" up to look like a sphere) gives finer control on how much detail we have on the sphere.
/// We do have to write a shader to ignore the seams (from the cube edges), but this should be easier to control than uniformly having increased detail on all faces.
/// (May want to re-examine this reasoning when the videos are over and think of alternatives).
/// </summary>
public class Planet : MonoBehaviour
{
    //256 squared is the max the mesh can be
    [Range(2, 256)]
    public int resolution = 10;

    /// <summary>
    /// Whether or not we want it to auto update everything when changed.
    /// </summary>
    public bool autoUpdate = true;

    /// <summary>
    /// Enum to only render one or all faces.
    /// </summary>
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;

    //Shape setting for the planet
    public ShapeSettings shapeSettings;

    //Color setting for the planet
    public ColorSettings colorSettings;

    //Since we can't save the state of the editor for the shape settings in the gui section since everything there is serialized (won't be saved), save here.
    [HideInInspector]
    public bool shapeSettingsFoldout;

    //Since we can't save the state of the editor for the color settings in the gui section since everything there is serialized (won't be saved), save here.
    [HideInInspector]
    public bool colorSettingsFoldout;

    //Shape generator
    ShapeGenerator shapeGenerator = new ShapeGenerator();

    //Color generator
    ColorGenerator colorGenerator = new ColorGenerator();

    //MeshFilter array for our six face meshes.
    //Note: MeshFilters hold the mesh data and pass it to meshRenderer
    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;

    /// <summary>
    /// TerrainFace array. Holds the six instances of terrainface.
    /// </summary>
    TerrainFace[] terrainFaces;

    private void OnValidate()
    {
        GeneratePlanet();
    }

    /// <summary>
    /// Initialize our faces
    /// </summary>
    private void Initialize()
    {
        //Init new shape generator
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);

        //If there aren't any meshFilters or the length is 0, we have to init the array
        if(meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];


        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        //Initialize the six meshes
        for(int i = 0; i < 6; i++)
        {
            if(meshFilters[i] == null)
            {
                //Create mesh object. Parent it to this transform just for organization
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                //Add mesh Renderer and meshfilters. Also apply a material.
                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);

            //If the rendermask is all OR the enum value is equivalent to its enum value in the render mask, then render
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    /// <summary>
    /// Called when everything is being changed.
    /// </summary>
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    /// <summary>
    /// When the shape settings are changed and we have to update
    /// </summary>
    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    /// <summary>
    /// When the color settings are changed and we have to update
    /// </summary>
    public void OnColorSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColors();
        }
    }

    /// <summary>
    /// Generate the actual mesh of the planet
    /// </summary>
    void GenerateMesh()
    {
        //Check all mesh filters
        for(int i = 0; i < 6; i++)
        {
            //If the mesh filter is active, construct mesh. otherwise ignore, it is being masked by faceRenderMask.
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
            }
        }

        //After generating all the meshes, elevationMinMax now has the min and max elevation. Pass to color generator.
        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    /// <summary>
    /// Generate colors of planet
    /// </summary>
    void GenerateColors()
    {
        colorGenerator.UpdateColors();
    }
}
