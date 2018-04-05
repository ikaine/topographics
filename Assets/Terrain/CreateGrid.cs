using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour {

    List<Vector3> points;
    public GameObject line;
    public Terrain terrainMain;
    private LineRenderer lineRenderer;
    private int y;

    public void Start()
    {
        setGrid();
    }

    public void setGrid()
    {
        Debug.Log(terrainMain.terrainData.size);
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();

        points = new List<Vector3>();
        y = Mathf.RoundToInt(this.gameObject.transform.position.y);
        int z = 0;
        int xValue;
        bool down = true;
        bool readyToIncrease = false;
        int counter = 0;
        Vector3 point;
        point.y = y;
        Vector3 terrainSize = terrainMain.terrainData.size;
        for (int x = 0; x <= terrainSize.x; x += 0)
        {
            point.x = x;
            point.z = z;
            Debug.Log(point.x + " " + point.y + " " + point.z);
            points.Add(point);

            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(points.Count - 1, point);

            if (counter == 0)
            {
                counter++;

                if (down)
                {
                    down = false;
                    z = Mathf.RoundToInt(terrainSize.z);
                }
                else
                {
                    down = true;
                    z = 0;
                }
                readyToIncrease = true;
            }
            else
            {
                if (readyToIncrease)
                {
                    counter = 0;
                    x += 200;
                    readyToIncrease = false;
                }
                else
                {
                    if (down)
                    {
                        down = false;
                        z = Mathf.RoundToInt(terrainSize.z);
                    }
                    else
                    {
                        down = true;
                        z = 0;
                    }
                }
            }
        }

        xValue = 0;
        readyToIncrease = true;
        counter = 1;
        for (int zValue = z; zValue <= Mathf.RoundToInt(terrainSize.z); zValue += 0)
        {
            point.x = xValue;
            point.z = zValue;
            Debug.Log(point.x + " " + point.y + " " + point.z);
            points.Add(point);

            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(points.Count - 1, point);

            if (counter == 0)
            {
                counter++;

                if (down)
                {
                    down = false;
                    xValue = Mathf.RoundToInt(terrainSize.x);
                }
                else
                {
                    down = true;
                    xValue = 0;
                }
                readyToIncrease = true;
            }
            else
            {
                if (readyToIncrease)
                {
                    counter = 0;
                    zValue += 200;
                    readyToIncrease = false;
                }
                else
                {
                    if (down)
                    {
                        down = false;
                        xValue = Mathf.RoundToInt(terrainSize.x);
                    }
                    else
                    {
                        down = true;
                        xValue = 0;
                    }
                }
            }
        }

    }
}
