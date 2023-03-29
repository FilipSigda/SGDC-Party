using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A static class used to get values from a bezier curve, as oppose to creating duplicate methods
/// </summary>
public static class BezierCurve
{
    /// <summary>
    /// returns a position on a bezier curve
    /// </summary>
    /// <param name="startPosition">the start point of the bezier curve</param>
    /// <param name="endPosition">the end point of the bezier curve</param>
    /// <param name="bezierOffset">the offset of the bezier position from the start</param>
    /// <param name="t">the percantage of the curve to get the position from. Must be between 0 and 1 (inclusive)</param>
    /// <returns>Vector3 in world space of the position on the curve</returns>
    public static Vector3 PositionOnCurve(Vector3 startPosition,Vector3 endPosition,Vector3 _bezierPoint, float t)
    {
        //right now its only the quadratic implementation. Might add more overloads, so this should stay as its own class
        return Mathf.Pow(1 - t, 2) * startPosition + 2 * t * (1 - t) * _bezierPoint + Mathf.Pow(t, 2) * endPosition;
    }

    public static float GetBezierLength(Vector3 startPosition, Vector3 endPosition, Vector3 _bezierPoint, int numberOfSteps)
    {
        float distance = 0f;
        float step = (1f / numberOfSteps);
        for (float i = 0; i < 1f; i += step)
        {
            distance = Vector3.Distance(PositionOnCurve(startPosition, endPosition, _bezierPoint, i), PositionOnCurve(startPosition, endPosition, _bezierPoint, i+step));
        }
        return distance;
    }


    public static void DrawBezierInDebug(Vector3 startPosition, Vector3 endPosition, Vector3 bezierOffset, int numberOfSteps, Color color)
    {
        float step = 1f / numberOfSteps;
        for(float i = 0; i < 1f; i += step)
        {
            Vector3 _current = PositionOnCurve(startPosition, endPosition, bezierOffset, i);
            Vector3 _next = PositionOnCurve(startPosition, endPosition, bezierOffset, i+ step);
            Debug.DrawLine(_current, _next, color);
        }
    }
    //overload
    public static void DrawBezierInDebug(Vector3 startPosition, Vector3 endPosition, Vector3 bezierOffset, int numberOfSteps) { DrawBezierInDebug(startPosition, endPosition, bezierOffset,numberOfSteps,Color.white); }
}
