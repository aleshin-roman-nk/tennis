using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveWithAcceleration
{
	public Transform movableObject;
	public Action OnStartMoving;
	public Action OnEndMoving;
	public Action<float> OnVelocityChanged;
	public IEnumerator update(Vector3 start, Vector3 end, float time)
	{
		float elapsedTime = 0f;
		Vector3 currentPosition = start;
		float initialVelocity = 0f;
		float acceleration = (2 * Vector3.Distance(start, end)) / (time * time);

		OnStartMoving?.Invoke();

		while (elapsedTime < time)
		{
			float t = elapsedTime / time; // progress as a ratio
			float velocity = initialVelocity + acceleration * elapsedTime; // calculate current velocity
			OnVelocityChanged?.Invoke(velocity);
			currentPosition += (end - start).normalized * velocity * Time.deltaTime;
			movableObject.transform.position = currentPosition;

			elapsedTime += Time.deltaTime;
			yield return null;
		}
		movableObject.transform.position = end; // ensure object reaches final position
	}
}
