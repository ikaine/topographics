using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

    //Draw line vars
    List<Vector3> points;
    public LineRenderer lineRenderer;
    /*public EdgeCollider2D edgeCol;*/

    public void updateLine(Vector3 mousePos)
    {
        if (points == null)
        {
            points = new List<Vector3>();
            setPoint(mousePos);
            return;
        }

        //Check if the mouse has moved enough for us tp insert new point.
        if (Vector3.Distance(points.Last(), mousePos) > .1f)
        {
            setPoint(mousePos);
        }
    }

    //Add the points to edgeCollider
  void setPoint(Vector3 point)
     {
         points.Add(point);

         lineRenderer.positionCount = points.Count;
         lineRenderer.SetPosition(points.Count - 1, point);

         //Add the points to edgeCollider
         if (points.Count > 1)
             points.ToArray();
        //edgeCol.points = points.ToArray();
    }
}
