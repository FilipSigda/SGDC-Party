using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class TileClass : MonoBehaviour
{

    public abstract GameObject[] nextTile { get; }
    public abstract GameObject[] previousTile { get; }
    
    public abstract Vector3 bezierOffset { get; }

    public Vector3 GetNextTilePos()
    {
        if (nextTile.Length == 1)
            return nextTile[0].transform.position;
        else/* if (nextTile.Length > 1)*/
            return handleIntersection();
        //else
        //    return Transform.position;
    }

    Vector3 handleIntersection()
    {  
        return Vector3.up;
    }

    private void OnDrawGizmosSelected()
    {
        //draws bezier curve and point when selected in the editor
        if (nextTile.Length > 0)
        {
            foreach (GameObject g in nextTile) { 
                Debug.DrawLine(transform.position, bezierOffset + transform.position, Color.blue);
                Gizmos.DrawSphere(bezierOffset + transform.position, 0.1f);
                BezierCurve.DrawBezierInDebug(transform.position, g.transform.position, bezierOffset + transform.position, 10, Color.green);
            }
        }
    }
}
