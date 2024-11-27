using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveConstantVelocity
{
	public Transform movableObject;
	public Action OnStartMoving;
	public Action OnEndMoving;

	public IEnumerator update(Vector3 start, Vector3 end, float speed)
	{
		float distance = Vector3.Distance(start, end);
		float totalTime = distance / speed;
		float elapsedTime = 0f;

		OnStartMoving?.Invoke();

		while (elapsedTime < totalTime)
		{
			movableObject.transform.position = Vector3.Lerp(start, end, elapsedTime / totalTime);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		movableObject.transform.position = end; // Ensure object reaches exact target position
	}
}
