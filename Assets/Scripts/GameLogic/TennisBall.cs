using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TennisBall : MonoBehaviour
{
	public float speed {  get; private set; }
	public Vector3 direction {  get; private set; }
	public bool movmentDefined {  get; private set; } = false;

	private Rigidbody rb;

	void Start()
	{
		//rb = GetComponent<Rigidbody>();
	}

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		if (transform.position.y < -5)
		{
			movmentDefined = false;
			//StopCoroutine(speedObserver);
			//speedObserver = null;
			gameObject.SetActive(false);
		}

		if (movmentDefined)
		{
			Debug.DrawRay(new Vector3(transform.position.x, 0, transform.position.z), direction, Color.red);
		}
	}

	public void Kick(Vector3 force)
	{
		rb.velocity = Vector3.zero;
		rb.AddForce(force, ForceMode.Impulse);

		//speedObserver = StartCoroutine(CalculateSpeed());

		StartCoroutine(catchBallMovement());
	}

	private Coroutine speedObserver = null;

	private IEnumerator CalculateSpeed()
	{
		Vector3 previousPosition = new Vector3(transform.position.x, 0, transform.position.z);
		float previousTime = Time.time;
		float speedXZ;

		while (true)
		{
			// Wait for 0.1 seconds
			yield return new WaitForSeconds(0.05f);

			// Capture the current position in the XZ plane
			Vector3 currentPosition = new Vector3(transform.position.x, 0, transform.position.z);

			// Calculate the distance traveled in the XZ plane
			float distanceXZ = Vector3.Distance(currentPosition, previousPosition);

			// Calculate the time elapsed between frames
			float currentTime = Time.time;
			float elapsedTime = currentTime - previousTime;

			// Speed = Distance / Time
			speedXZ = distanceXZ / elapsedTime;

			// Output the speed in the XZ plane
			//Debug.Log($"me zx speed {speedXZ} | RB zx speed {new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude} | absolute speed {rb.velocity.magnitude}");
			Debug.Log($"distance {distanceXZ} | time {elapsedTime}");

			// Update the previous position for the next calculation
			previousPosition = currentPosition;
			previousTime = currentTime;
		}
	}

	private IEnumerator catchBallMovement()
	{
		float delta = 0;

		var positionA = transform.position;
		var positionB = positionA;

		float time1 = Time.time;

		while (delta == 0)
		{
			positionB = transform.position;
			delta = Vector3.Distance(positionB, positionA);
			yield return null;
		}

		//var deltaTime = Time.time - time1;

		//positionB = new Vector3(positionB.x, 0, positionB.z);
		//positionA = new Vector3(positionA.x, 0, positionA.z);

		//direction = (positionB - positionA).normalized;

		//var distance = (positionB - positionA).magnitude;
		//speed = distance / deltaTime;

		direction = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized;
		speed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;

		movmentDefined = true;
	}
}
