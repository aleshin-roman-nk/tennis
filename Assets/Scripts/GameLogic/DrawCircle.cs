using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
	public LineRenderer lineRenderer;
	public int segments = 100;
	public float radius = 5f;

	void Start()
	{
		drawCircle();
	}

	void drawCircle()
	{
		lineRenderer.positionCount = segments + 1;
		lineRenderer.useWorldSpace = false;

		float angle = 0f;
		for (int i = 0; i <= segments; i++)
		{
			float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
			float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

			lineRenderer.SetPosition(i, new Vector3(x, y, 0));
			angle += 360f / segments;
		}
	}
}
