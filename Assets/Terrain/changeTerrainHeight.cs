using System;
using System.Collections;
using UnityEngine;
public class changeTerrainHeight : MonoBehaviour {

    public Terrain TerrainMain;
    public LineRenderer line;
    void Update()
    {

    }
    void OnGUI()
    {
        //Get the terrain heightmap width and height.
        int xRes = TerrainMain.terrainData.heightmapWidth;
        int yRes = TerrainMain.terrainData.heightmapHeight;

        //GetHeights - gets the heightmap points of the tarrain. Store them in array
        float[,] heights = TerrainMain.terrainData.GetHeights(0, 0, xRes, yRes);

        if (GUI.Button(new Rect(30, 30, 200, 30), "Change terrain Height"))
        {

            for (int i = 0; i < (yRes / 2); i++)
            {
                for (int j = 0; j < (xRes / 2); j++)
                {
                    heights[i, j] = 0.5f; // 0-1. 1 being the max possible height 
                }
            }
            //manipulate the height data
            heights[10, 10] = 0.5f; // 0-1. 1 being the max possible height 

            //SetHeights to change the terrain height.
            TerrainMain.terrainData.SetHeights(0, 0, heights);

        }
        if (GUI.Button(new Rect(30, 60, 200, 30), "Add circle"))
        {
            int cRadius = 20;
            int cPosX = 100;
            int cPosY = 100;
            float height = 0.25f;

            for (int i = 0; i < yRes; i++)
            {
                for (int j = 0; j < xRes; j++)
                {
                    /*if (i <= Math.Sqrt(cRadius*cRadius - (j - cPosY)*(j - cPosY)) && j <= Math.Sqrt(cRadius * cRadius - (i - cPosX) * (i - cPosX)))
                    {
                        heights[i, j] = 0.5f; // 0-1. 1 being the max possible height 
                    }*/
                    if ((cRadius * cRadius) > ((i - cPosX) * (i - cPosX) + (j - cPosY) * (j - cPosY)))
                    {
                        heights[j, i] = height; // 0-1. 1 being the max possible height
                    }
                }
            }

            //SetHeights to change the terrain height.
            TerrainMain.terrainData.SetHeights(0, 0, heights);

        }

        if (GUI.Button(new Rect(30, 90, 200, 30), "Line points"))
        {
            /* Set the positions to array "positions" */
            Vector3[] positions = new Vector3[line.positionCount];
            line.GetPositions(positions);
            


            float height = 0.05f;

            for (int i = 0; i < line.positionCount; i++)
            {
                heights[Mathf.RoundToInt(positions[i].z), Mathf.RoundToInt(positions[i].x)] = height;
            }

            //SetHeights to change the terrain height.
            TerrainMain.terrainData.SetHeights(0, 0, heights);

        }

        if (GUI.Button(new Rect(30, 120, 200, 30), "Line fill"))
        {
            /* Set the positions to array "positions" */
            Vector3[] positions = new Vector3[line.positionCount];
            line.GetPositions(positions);
            
            float height = 0.25f;

            /* Find the reactangle the shape is in! The sides of the rectangle are based on the most-top, -right, -bottom and -left vertex.
            ** Use Rect.Contains in Unity API and this website: https://answers.unity.com/questions/850138/how-do-i-find-the-top-of-a-mesh.html
            */

            float ftop = float.NegativeInfinity;
            float fright = float.NegativeInfinity;
            float fbottom = Mathf.Infinity;
            float fleft = Mathf.Infinity;
            for (int i = 0; i < line.positionCount; i++)
            {
                if(ftop < positions[i].z)
                {
                    ftop = positions[i].z;
                }
                if (fright < positions[i].x)
                {
                    fright = positions[i].x;
                }
                if (fbottom > positions[i].z)
                {
                    fbottom = positions[i].z;
                }
                if (fleft > positions[i].x)
                {
                    fleft = positions[i].x;
                }
            }
            int top = Mathf.RoundToInt(ftop);
            int right = Mathf.RoundToInt(fright);
            int bottom = Mathf.RoundToInt(fbottom);
            int left = Mathf.RoundToInt(fleft);
            
            int terrainChangeXmax = right - left;
            int terrainChangeZmax = top - bottom;

            Debug.Log(left); //135
            Debug.Log(bottom); //102
            Debug.Log(right); //356
            Debug.Log(top); //359
            Debug.Log(terrainChangeXmax); //221
            Debug.Log(terrainChangeZmax); //257
            float[,] shapeHeights = TerrainMain.terrainData.GetHeights(left, bottom, terrainChangeXmax, terrainChangeZmax);

            
            bool raise = true;
            int collisionRadius = 0;
            int xCrd = 0;
            int zCrd = 0;

            /* Loop through all points of the shape perimeter */
            for (int i = 0; i < line.positionCount; i++)
            {
                for(int j = 0; j < (terrainChangeZmax - 1); j++)
                {
                    xCrd = right - Mathf.RoundToInt(positions[i].x);
                    zCrd = Mathf.RoundToInt(positions[i].z) - bottom;
                    if ((j - collisionRadius) < zCrd && zCrd <= (j + collisionRadius))
                    {
                        raise = raise ? false : true;
                    }
                    Debug.Log("point: " + i + "; z: " + xCrd + "; x: " + j + "; raise: " + raise);
                    

                    if (raise)
                    {
                        shapeHeights[j, xCrd] = height;
                    }
                    /*TerrainMain.terrainData.SetHeightsDelayLOD(left, bottom, shapeHeights);*/

                }
            }

            //SetHeights to change the terrain height.
            TerrainMain.terrainData.SetHeightsDelayLOD(left, bottom, shapeHeights);
            TerrainMain.ApplyDelayedHeightmapModification();
        }
    }
}
