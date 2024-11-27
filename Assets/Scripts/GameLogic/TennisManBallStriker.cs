using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisManBallStriker : MonoBehaviour
{
	[SerializeField] private HumanPlayer humanPlayer;

	private void Start()
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		// Check if the object that entered the trigger is in the "ball" layer
		if (other.CompareTag("Ball"))
		{
			var rb = other.gameObject.GetComponent<Rigidbody>();
			var ball = other.gameObject.GetComponent<TennisBall>();

			var pos = humanPlayer.Next();

			ball.Kick(pos.transform.position, KickedBy.ai, 14);
		}
	}
}
