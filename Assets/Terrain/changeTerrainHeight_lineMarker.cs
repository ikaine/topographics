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

        //Trigger line area raiser
        if (GUI.Button(new Rect(30, 30, 200, 30), "Line fill"))
        {
            /* Set the positions to array "positions" */
            Vector3[] positions = new Vector3[line.positionCount];
            line.GetPositions(positions);

            float height = 0.01f; // define the height of the affected verteces of the terrain

            /* Find the reactangle the shape is in! The sides of the rectangle are based on the most-top, -right, -bottom and -left vertex. */
            float ftop = float.NegativeInfinity;
            float fright = float.NegativeInfinity;
            float fbottom = Mathf.Infinity;
            float fleft = Mathf.Infinity;
            for (int i = 0; i < line.positionCount; i++)
            {
                //find the outmost points
                if (ftop < positions[i].z)
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

            int terrainXmax = right - left; // the rightmost edge of the terrain
            int terrainZmax = top - bottom; // the topmost edge of the terrain 

            float[,] shapeHeights = TerrainMain.terrainData.GetHeights(left, bottom, terrainXmax, terrainZmax);

            Vector2 point; //Create a point Vector2 point to match the shape

            /* Loop through all points in the rectangle surrounding the shape */
            for (int i = 0; i < terrainZmax; i++)
            {
                point.y = i + bottom; //Add off set to the element so it matches the position of the line
                for (int j = 0; j < terrainXmax; j++)
                {
                    point.x = j + left; //Add off set to the element so it matches the position of the line
                    if (InsidePolygon(point, bottom))
                    {
                        shapeHeights[i, j] += height; // set the height value to the terrain vertex
                    }
                }
            }

            //SetHeights to change the terrain height.
            TerrainMain.terrainData.SetHeightsDelayLOD(left, bottom, shapeHeights);
            TerrainMain.ApplyDelayedHeightmapModification();
        }
    }

    //Checks if the given vertex is inside the the shape.
    bool InsidePolygon(Vector2 p, int terrainZmax)
    {
        // Assign the points that define the outline of the shape
        Vector3[] positions = new Vector3[line.positionCount];
        line.GetPositions(positions);

        int count = 0;
        Vector2 p1, p2;
        int n = positions.Length;

        // Find the lines that define the shape
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

            // check if the given point p intersects with the lines that form the outline of the shape.
            if (LinesIntersect(p1, p2, p, terrainZmax))
            {
                count++;
            }
        }

        // the point is inside the shape when the number of line intersections is an odd number
        if (count % 2 == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Function that checks if two lines intersect with each other
    bool LinesIntersect(Vector2 A, Vector2 B, Vector2 C, int terrainZmax)
    {
        Vector2 D = new Vector2(C.x, terrainZmax - 1);
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

}

