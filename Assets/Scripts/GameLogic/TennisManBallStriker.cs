using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisManBallStriker : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		// Check if the object that entered the trigger is in the "ball" layer
		if (other.gameObject.layer == LayerMask.NameToLayer("ball"))
		{
			var rb = other.gameObject.GetComponent<Rigidbody>();
			rb.velocity = -rb.velocity * 1.5f;
		}
	}
}
