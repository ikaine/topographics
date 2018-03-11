using UnityEngine;
using System.Collections;

public class Tracer : MonoBehaviour
{
    public Terrain TerrainMain;
    int xPos;
    int zPos;
    int cRadius = 10;
    float height = 0.05f;
    Plane objPlane;
    float rayDistance;

    // Update is called once per frame
    void Update()
    {
        if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0)))
        {
            objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (objPlane.Raycast(mRay, out rayDistance))
            {
                this.transform.position = mRay.GetPoint(rayDistance);
                xPos = (int) mRay.GetPoint(rayDistance).x;
                zPos = (int) mRay.GetPoint(rayDistance).z;

                /* Height data is adjusted to the circle shaped "Brush" and it's centered position */
                float[,] pointerHeights = TerrainMain.terrainData.GetHeights((xPos-cRadius), (zPos - cRadius), (2* cRadius), (2 * cRadius));
                for (int i = 0; i < (2 * cRadius); i++)
                {
                    for (int j = 0; j < (2 * cRadius); j++)
                    {
                        if ((cRadius * cRadius) >= ((i - cRadius) * (i - cRadius) + (j - cRadius) * (j - cRadius)))
                        {
                            pointerHeights[i, j] = height;
                        }
                    }
                }
                TerrainMain.terrainData.SetHeightsDelayLOD((xPos - cRadius), (zPos - cRadius), pointerHeights);
                TerrainMain.ApplyDelayedHeightmapModification();
            }

            
        }
    }
}
