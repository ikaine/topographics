using System;
using System.Collections;
using UnityEngine;
public class changeTerrainHeight : MonoBehaviour {

    public Terrain TerrainMain;
    public LineRenderer line;


    bool LinesIntersect(Vector2 A, Vector2 B, Vector2 C, int terrainZmax)
    {
        Vector2 D = new Vector2(C.x, terrainZmax + 1);
        Vector2 CmP = new Vector2(C.x - A.x, C.y - A.y);
        Vector2 r = new Vector2(B.x - A.x, B.y - A.y);
        Vector2 s = new Vector2(D.x - C.x, D.y - C.y);

        float CmPxr = CmP.x * r.y - CmP.y * r.x;
        float CmPxs = CmP.x * s.y - CmP.y * s.x;
        float rxs = r.x * s.y - r.y * s.x;

        if (CmPxr == 0f)
        {
            // Lines are collinear, and so intersect if they have any overlap

            return ((C.x - A.x < 0f) != (C.x - B.x < 0f))
                || ((C.y - A.y < 0f) != (C.y - B.y < 0f));
        }

        if (rxs == 0f)
            return false; // Lines are parallel.

        float rxsr = 1f / rxs;
        float t = CmPxs * rxsr;
        float u = CmPxr * rxsr;

        return (t >= 0f) && (t <= 1f) && (u >= 0f) && (u <= 1f);
    }

    bool InsidePolygon(Vector2 p, int terrainZmax)
    {
        Vector3[] positions = new Vector3[line.positionCount];
        line.GetPositions(positions);

        int count = 0;
        Vector2 p1, p2;
        int n = positions.Length;

        for (int i = 0; i < n; i++)
        {
            p1.y = positions[i].z;// - p.y;
            p1.x = positions[i].x;// - p.x;
            if (i != n - 1)
            {
                p2.y = positions[(i + 1)].z;// - p.y;
                p2.x = positions[(i + 1)].x;// - p.x;
            }
            else
            {
                p2.y = positions[0].z;// - p.y;
                p2.x = positions[0].x;// - p.x;
            }
            if (LinesIntersect(p1, p2, p, terrainZmax))
            {
                count++;
            }
        }

        if (count % 2 == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

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
            
            float height = 0.10f;

            /* Find the reactangle the shape is in! The sides of the rectangle are based on the most-top, -right, -bottom and -left vertex.
            ** Use Rect.Contains in Unity API and this website: https://answers.unity.com/questions/850138/how-do-i-find-the-top-of-a-mesh.html
            */

            float ftop = float.NegativeInfinity;
            float fright = float.NegativeInfinity;
            float fbottom = Mathf.Infinity;
            float fleft = Mathf.Infinity;
            for (int i = 0; i < line.positionCount; i++)
            {
                //find the outmost points
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
            
            int terrainXmax = right - left;
            int terrainZmax = top - bottom;

            /*Debug.Log(left); //135
            Debug.Log(bottom); //102
            Debug.Log(right); //356
            Debug.Log(top); //359
            Debug.Log(terrainXmax); //221
            Debug.Log(terrainZmax); //257*/
            float[,] shapeHeights = TerrainMain.terrainData.GetHeights(left, bottom, terrainXmax, terrainZmax);

            Vector2 point;
            /* Loop through all points of the shape perimeter */
            for (int i = 0; i < terrainZmax; i++)
            {
                point.y = i + 102;
                for (int j = 0; j < terrainXmax; j++)
                {
                    point.x = j + 135;
                    if (InsidePolygon(point, bottom))
                    {
                        shapeHeights[i, j] = height;
                    }
                }
            }

            //SetHeights to change the terrain height.
            TerrainMain.terrainData.SetHeightsDelayLOD(left, bottom, shapeHeights);
            TerrainMain.ApplyDelayedHeightmapModification();
        }
    }
}
