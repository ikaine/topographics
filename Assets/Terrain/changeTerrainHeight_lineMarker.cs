using System;
using System.Collections;
using UnityEngine;
public class changeTerrainHeight_lineMarker : MonoBehaviour
{

    public Terrain TerrainMain;
    public LineRenderer line;

    void OnGUI()
    {
        //Get the terrain heightmap width and height.
        int xRes = TerrainMain.terrainData.heightmapWidth;
        int yRes = TerrainMain.terrainData.heightmapHeight;

        //GetHeights - gets the heightmap points of the tarrain. Store them in array
        float[,] heights = TerrainMain.terrainData.GetHeights(0, 0, xRes, yRes);
        
        //Trigger line point raiser
        if (GUI.Button(new Rect(30, 30, 200, 30), "Line points"))
        {
            /* Set the positions to array "positions" */
            Vector3[] positions = new Vector3[line.positionCount];
            line.GetPositions(positions);


            /* use this height to the affected terrain verteces */
            float height = 0.05f;

            for (int i = 0; i < line.positionCount; i++)
            {
                /* Assign height data */
                heights[Mathf.RoundToInt(positions[i].z), Mathf.RoundToInt(positions[i].x)] = height;
            }

            //SetHeights to change the terrain height.
            TerrainMain.terrainData.SetHeights(0, 0, heights);

        }
        
    }
}
