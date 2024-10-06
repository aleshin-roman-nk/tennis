using System.Collections.Generic;
using UnityEngine;

public class NonOverlappingCircles : MonoBehaviour
{
	public float circleRadius = 10f; // Radius of the circular area
	public Vector3 area = new Vector3(1, 1, 1);
	public float pointRadius = 0.1f; // Radius of each point
	public int numberOfPoints = 20;  // Number of points to generate

	private List<Vector3> points;

	void Start()
	{
		points = new List<Vector3>();
		GeneratePoints();
		DrawPoints();
	}

	void GeneratePoints()
	{
		int attempts = 0;
		while (points.Count < numberOfPoints && attempts < numberOfPoints * 10)
		{
			Vector3 newPoint = GenerateRandomPointInCircle(circleRadius);
			if (IsPointValid(newPoint))
			{
				points.Add(newPoint);
			}
			attempts++;
		}
	}

	Vector3 GenerateRandomPointInCircle(float radius)
	{
		float angle = Random.Range(0, 2 * Mathf.PI);
		float distance = Random.Range(0, radius - pointRadius);
		float x = Mathf.Cos(angle) * distance;
		float z = Mathf.Sin(angle) * distance;
		return new Vector3(x, 0, z);
	}

	bool IsPointValid(Vector3 newPoint)
	{
		foreach (Vector3 point in points)
		{
			if (Vector3.Distance(newPoint, point) < 2 * pointRadius)
			{
				return false;
			}
		}
		return true;
	}

	void DrawPoints()
	{
		foreach (Vector3 point in points)
		{
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.transform.parent = transform;
			sphere.transform.localPosition = point;
			sphere.transform.localScale = Vector3.one * pointRadius * 2;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		//Gizmos.DrawWireSphere(transform.position, circleRadius);
		Gizmos.DrawWireCube(transform.position, area);
	}
}
