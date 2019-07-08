using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// One TerrainFace mesh is one of the six sides of the blown up cube. See Planet script for more details.
/// </summary>
public class TerrainFace
{
    /// <summary>
    /// Shape generator. Tells us where to place points
    /// </summary>
    ShapeGenerator shapeGenerator;

    /// <summary>
    /// Our own mesh.
    /// </summary>
    Mesh mesh;

    /// <summary>
    /// Resolution level of mesh. Controls how much detail we have.
    /// </summary>
    int resolution;

    /// <summary>
    /// Which direction we are facing. This is the up direction of this face, the normal direction.
    /// </summary>
    Vector3 localUp;

    /// <summary>
    /// The other axis in relation to localUp
    /// </summary>
    Vector3 axisA;

    /// <summary>
    /// The other axis in relation to both localUp and axisA
    /// </summary>
    Vector3 axisB;

    /// <summary>
    /// Constructor for terrainFace
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="resolution"></param>
    /// <param name="localUp"></param>
    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        //AxisA is just a swap of the x,y and z of localUp.
        axisA = new Vector3(localUp.y, localUp.z, localUp.x);

        //AxisB is the cross product of axisA and local up.
        //Cross product of two vectors gives the normal of the plane formed by the two vectors.
        axisB = Vector3.Cross(localUp, axisA);
    }

    /// <summary>
    /// Constructs mesh 
    /// </summary>
    public void ConstructMesh()
    {
        //Number of vertices is resolution squared. (We want a sphere with square faces. Could be different for weird shapes).
        Vector3[] vertices = new Vector3[resolution * resolution];

        //Number of triangles in our mesh
        //If resolution is 4, we have 4 vertices by 4 vertices. Every square made of the vertices contains two triangles.
        //Thus to calculate this is 
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];

        //Counts index of the triangle we're making
        int triIndex = 0;

        for(int y = 0; y < resolution; y++)
        {
            for(int x = 0; x < resolution; x++)
            {
                //Index of the new vertices. Same as incrementing a variable i every time.
                int index = x + y * resolution;

                //This gives us the percent completion of each loop
                Vector2 percent = new Vector2(x, y) / (resolution - 1);

                //Calculate a new point given how far through the loop of vertice numbers we are.
                //Example: at 0,0, pointOnUnitCube will be (-1, 1, -1) (in the grid of our axis)
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;

                //Not the best way according to episode one, but will be a good  guess
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                //Set the new vertice
                vertices[index] = shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);
                
                //Note: Triangles should be formed clockwise since this is how they determine direction the face faces.
                //If r = 4. An example triangle is 0, 5, 4, and 0, 1, 5.
                //i, i + r + 1, i + r for the first one
                //i, i+1, i + r + 1
                //Given this:
                //  0   1   2   3
                //  4   5   6   7
                //  8   9   10  11
                //  12  13  14  15

                //Forms the two triangles that make up one unit face.
                //We can't allow x == resolution because it will form a triangle outside of our resolution size.
                //We also don't allow it when y is close to resolution minus 1 for the same reason
                if( x != resolution - 1 && y != resolution - 1)
                {
                    //First triangle
                    triangles[triIndex] = index;
                    triangles[triIndex + 1] = index + resolution + 1;
                    triangles[triIndex + 2] = index + resolution;

                    //Second triangle
                    triangles[triIndex + 3] = index;
                    triangles[triIndex + 4] = index + 1;
                    triangles[triIndex + 5] = index + resolution + 1;

                    //Increment correctly
                    triIndex += 6;
                }
            }
        }
        //Now assign all the vertices and triangles.
        //Clear data first. If there is an older resolution in place it will throw an error if we reference
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        //Recalculate normals with our new meshes
        mesh.RecalculateNormals();
    }
}
