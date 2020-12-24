using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Vector3 _CaculateCubicCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //B(t) = (1 - t)3.P0 [+] 3(1 - t)2.t.P1 [+] 3.(1 - t).t2.P2 [+] t3.P3 , Condition 0 < t < 1
        //          u               u                  u
        float u = (1 - t);
        float uu = u * u;
        float tt = t * t;
        float uuu = uu * u;
        float ttt = tt * t;
        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;
        return p;
    }
    public void _DrawCubicCurveInList(int numPoints,Transform[] controlP,LineRenderer line,bool isDrawLine,List<Vector3> setPos)
    {
        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (float)numPoints;
            Vector3 caculateVector = _CaculateCubicCurve(t, controlP[0].transform.position,
                controlP[1].transform.position,
                controlP[2].transform.position,
                controlP[3].transform.position);
            setPos.Add(caculateVector);
            if(isDrawLine)
                line.SetPosition(i, caculateVector);
        }
    }
}
