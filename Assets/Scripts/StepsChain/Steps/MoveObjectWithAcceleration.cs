using System;
using System.Collections;
using UnityEngine;

public class MoveObjectWithAcceleration
{
	public Transform movableObject;
	public Action OnStartMoving;
	public Action OnEndMoving;

	public IEnumerator update(Vector3 startPosition, Vector3 endPosition, float v1, float v2, float accelerationDuration)
	{
		var totalDistance = Vector3.Distance(startPosition, endPosition);

		float elapsedTime = 0f;
		float distanceCovered = 0f;
		Vector3 direction = (endPosition - startPosition).normalized;  // Direction from A to B

		OnStartMoving?.Invoke();

		// Acceleration phase: move from v1 to v2 in 'time' seconds
		while (elapsedTime < accelerationDuration)
		{
			// Calculate the current velocity based on time passed
			float currentVelocity = Mathf.Lerp(v1, v2, elapsedTime / accelerationDuration);

			// Calculate the distance covered based on the current velocity
			float step = currentVelocity * Time.deltaTime;
			distanceCovered += step;

			// Ensure the object does not overshoot the final point during acceleration
			if (distanceCovered >= totalDistance)
			{
				movableObject.position = endPosition;
				OnEndMoving?.Invoke();
				yield break;  // Stop the coroutine if we reach or exceed the target
			}

			// Move the object
			movableObject.position = startPosition + direction * distanceCovered;

			// Update time and wait for the next frame
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		// Constant velocity phase: move with constant velocity V2
		while (distanceCovered < totalDistance)
		{
			float step = v2 * Time.deltaTime;
			distanceCovered += step;

			// Ensure the object does not overshoot the final point during constant velocity
			if (distanceCovered >= totalDistance)
			{
				movableObject.position = endPosition;
				OnEndMoving?.Invoke();
				yield break;  // Stop the coroutine once we reach the target
			}

			// Move the object
			movableObject.position = startPosition + direction * distanceCovered;

			// Wait for the next frame
			yield return null;
		}
	}
}
