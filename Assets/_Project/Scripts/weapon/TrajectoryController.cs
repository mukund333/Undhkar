using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryController : MonoBehaviour
{
    [Header("Line renderer veriables")]
    public LineRenderer line;
    [Range(2, 30)]
    public int resolution;

    [Header("Formula variables")]
    public Vector2 velocity;
    public float yLimit;
    private float g;

    [Header("Linecast variables")]
    [Range(2, 30)]
    public int linecastResolution;
    public LayerMask canHit;

	List<Vector2> curveXY = new List<Vector2>();
	[Header("curve")]
	[Range(0,8)]
	public int i =0;

    private void Start()
    {
		curveXY.Add(new Vector2(-180f,5f));
		curveXY.Add(new Vector2(-110.3f,25.6f));
		curveXY.Add(new Vector2(-90f,39.9f));
		curveXY.Add(new Vector2(-78.7f,53.2f));
		curveXY.Add(new Vector2(-59.7f,58.8f));
		curveXY.Add(new Vector2(-46.7f,64.3f));
		curveXY.Add(new Vector2(-39.9f,75.1f));
		curveXY.Add(new Vector2(-32.4f,91.9f));
		curveXY.Add(new Vector2(-21.6f,119.9f));
        g = Mathf.Abs(Physics2D.gravity.y);
    }

    private void Update()
    {
        RenderArc();
		velocity = curveXY[i];
    }

    private void RenderArc()
    {
        line.positionCount = resolution + 1;
        line.SetPositions(CalculateLineArray());
    }

    private Vector3[] CalculateLineArray()
    {
        Vector3[] lineArray = new Vector3[resolution + 1];

        var lowestTimeValue = MaxTimeX() / resolution;

        for (int i = 0; i < lineArray.Length; i++)
        {
            var t = lowestTimeValue * i;
            lineArray[i] = CalculateLinePoint(t);
        }

        return lineArray;
    }

    private Vector2 HitPosition()
    {
        var lowestTimeValue = MaxTimeY() / linecastResolution;

        for (int i = 0; i < linecastResolution + 1; i++)
        {
            var t = lowestTimeValue * i;
            var tt = lowestTimeValue * (i + 1);

            var hit = Physics2D.Linecast(CalculateLinePoint(t), CalculateLinePoint(tt), canHit);

            if (hit)
                return hit.point;
        }

        return CalculateLinePoint(MaxTimeY());
    }

    private Vector3 CalculateLinePoint(float t)
    {
        float x = velocity.x * t;
        float y = (velocity.y * t) - (g * Mathf.Pow(t, 2) / 2);
        return new Vector3(x + transform.position.x, y + transform.position.y);
    }

    private float MaxTimeY()
    {
        var v = velocity.y;
        var vv = v * v;

        var t = (v + Mathf.Sqrt(vv + 2 * g * (transform.position.y - yLimit))) / g;
        return t;
    }

    private float MaxTimeX()
    {
        var x = velocity.x;
        if(x == 0)
        {
            velocity.x = 000.1f;
            x = velocity.x;
        }
        
        var t = (HitPosition().x - transform.position.x) / x;
        return t;
    }
}