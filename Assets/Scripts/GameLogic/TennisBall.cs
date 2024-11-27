using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TouchScript.Examples.RawInput;
using Unity.VisualScripting;
using UnityEngine;

public class TennisBall : MonoBehaviour
{
	public float speedXZ {  get; private set; }
	public Vector3 directionXZ {  get; private set; }

	private Rigidbody rb;
	private Vector3 initialVelocity = Vector3.zero;
	private Vector3 gravityEffect; // Store the scaled gravity effect

	public KickedBy whoKicked { get; private set; }
	public bool hitNet { get; private set; }

	void Start()
	{
		gravityEffect = Physics.gravity;
	}

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private float oldSpeed = 0;

	void Update()
	{
		speedXZ = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
		directionXZ = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized;

		//if(hightlightImage.gameObject.activeSelf)
		//{
		//	Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

		//	// Convert the screen position to a local position on the Canvas
		//	RectTransformUtility.ScreenPointToLocalPointInRectangle(
		//		hightlightImage.parent as RectTransform,
		//		screenPosition,
		//		Camera.main,
		//		out Vector2 localPosition
		//	);

		//	// Update the UI element's position
		//	hightlightImage.localPosition = localPosition;
		//}

		//if (elapsedLifeTime >= lifeTime)
		//{
		//	movmentDefined = false;
		//	gameObject.SetActive(false);
		//}

		//if (movmentDefined)
		//{
		//	speedXZ = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
		//	directionXZ = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized;
		//	if (oldSpeed != speedXZ)
		//	{
		//		OnVelocityXZChanged?.Invoke(speedXZ * directionXZ);
		//		points.Add(rb.position);
		//	}
		//	oldSpeed = speedXZ;
		//	Debug.DrawRay(new Vector3(transform.position.x, 0.1f, transform.position.z), directionXZ, Color.red);
		//}

//		elapsedLifeTime += Time.deltaTime;
	}

	public void RestoreMovement()
	{
		restoreMovement();
	}

	public void SlowdownMovement(float sdFactor)
	{
		changeMovement(sdFactor);
	}

	private void OnCollisionEnter(Collision collision)
	{
		// When hitting the ground, maintain XZ direction
		if (collision.gameObject.CompareTag("ground"))
		{
			//Debug.Log($"hit grount {rb.velocity}");
			rb.velocity = new Vector3(initialVelocity.x, rb.velocity.y, initialVelocity.z);
		}
		// When hitting the ground, maintain XZ direction
		else if (collision.gameObject.CompareTag("TennisBallNet"))
		{
			hitNet = true;
		}
	}

	private void changeMovement(float factor)
	{
		rb.useGravity = false;
		rb.velocity *= factor;
	}

	private void restoreMovement()
	{
		rb.velocity = initialVelocity.magnitude * rb.velocity.normalized;
		rb.useGravity = true;
	}

	public HumanBallPoint humanBallPoint { get; private set; }

	public IEnumerator Kick(Vector3 targetPoint, KickedBy whoKicked, float speed, HumanBallPoint hpTarget = null)
	{
		this.whoKicked = whoKicked;
		this.hitNet = false;
		humanBallPoint = hpTarget;

		rb.useGravity = true;
		rb.velocity = Vector3.zero;

		LaunchBallWithSpeed(rb, transform.position, targetPoint, speed);

		yield return null;
		initialVelocity = rb.velocity;
	}

	private void LaunchBallWithSpeed(Rigidbody rb, Vector3 startPoint, Vector3 targetPoint, float speed)
	{
		// Gravity (from Unity's physics system)
		Vector3 gravity = Physics.gravity;

		// Horizontal displacement (XZ-plane)
		Vector3 horizontalDisplacement = new Vector3(targetPoint.x - startPoint.x, 0, targetPoint.z - startPoint.z);
		float horizontalDistance = horizontalDisplacement.magnitude;

		// Calculate time of flight based on speed
		float timeToTarget = horizontalDistance / speed;

		// Calculate horizontal velocity
		Vector3 horizontalVelocity = horizontalDisplacement.normalized * speed;

		// Calculate vertical velocity
		float verticalDisplacement = targetPoint.y - startPoint.y;
		float verticalVelocity = (verticalDisplacement / timeToTarget) - (0.5f * gravity.y * timeToTarget);

		// Combine velocities
		Vector3 initialVelocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);

		// Apply force or velocity
		rb.velocity = initialVelocity; // Directly set velocity
									   // OR, for AddForce:
									   // rb.AddForce(initialVelocity * rb.mass, ForceMode.Impulse);
	}
}

public enum KickedBy { rest = 0, human = 1, ai = 2 }
