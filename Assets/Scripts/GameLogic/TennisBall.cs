using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisBall : MonoBehaviour
{
	public float forceAmount = 10f;  // Force applied to the ball
	public float bounceForce = 5f;   // Vertical force for bouncing
	public float maxSpeed = 20f;     // Max speed limit
	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		HandleInput();
		LimitSpeed();
	}

	public void Kick(Vector3 force)
	{
		rb.velocity = Vector3.zero;
		rb.AddForce(force, ForceMode.Impulse);
	}

	void HandleInput()
	{
		// Apply forward force (for example, serve or hit)
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Vector3 hitDirection = -transform.forward; // Forward direction for hit
			rb.AddForce(hitDirection * forceAmount, ForceMode.Impulse);
		}

		// Bounce simulation (for example, after hitting the ground)
		if (Input.GetKeyDown(KeyCode.B))
		{
			rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
		}
	}

	void LimitSpeed()
	{
		// Limit the velocity to maxSpeed to simulate realistic ball behavior
		if (rb.velocity.magnitude > maxSpeed)
		{
			rb.velocity = rb.velocity.normalized * maxSpeed;
		}
	}
}
