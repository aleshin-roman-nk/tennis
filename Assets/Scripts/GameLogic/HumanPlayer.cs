using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.UI;

public class HumanPlayer : MonoBehaviour
{
	[SerializeField] private AIPlayer tennisMan;
	[SerializeField] private QTEAskHumanManager qteAskHumanManager;
	[SerializeField] private TennisBall ball;
	[SerializeField] private float kickingForce = 10f;
	[SerializeField] private float zPosBallLandingBeforeAI;

	[SerializeField] private HumanBallPoint[] ballTargets;
	[SerializeField] private HumanBallPoint[] servingPoints;

	private RandomList<HumanBallPoint> ballTargetRandomList;

	private Vector3 hitDirection = Vector3.zero;

	private void Start()
	{
		ballTargetRandomList = new RandomList<HumanBallPoint>(ballTargets, canRepeatTwice: false);

		qteAskHumanManager.AskUserFailed += QteAskHumanManager_AskUserFailed;
		qteAskHumanManager.UserMadeSolution += QteAskHumanManager_UserMadeSolution;
		qteAskHumanManager.AskUserStarted += QteAskHumanManager_AskUserStarted;
	}

	public void ServeBall()
	{

	}

	private void QteAskHumanManager_AskUserStarted()
	{
		_ballTriggeredPoint = false;
	}

	private void QteAskHumanManager_UserMadeSolution(string obj, HumanBallPoint point)
	{
		Vector3 dir = Vector3.zero;

		if (obj.Equals("up"))
		{
			dir = kickDirection(new Vector3(0, 0, -1), 0, -2, 2);
		}
		else if (obj.Equals("down"))
		{
			dir = kickDirection(new Vector3(0, 0, -1), 0, -2, 2);
		}
		else if (obj.Equals("left"))
		{
			dir = kickDirection(new Vector3(0, 0, -1), -10, -2, 2);
		}
		else if (obj.Equals("right"))
		{
			dir = kickDirection(new Vector3(0, 0, -1), 10, -2, 2);
		}

		Vector3 targetPos = findPositionAcrossZlevelXZ(
			point.transform.position,
			dir,
			tennisMan.transform.position.z - zPosBallLandingBeforeAI);

		StartCoroutine(playBackCoroutine(targetPos, point.transform.position, 0.3f));
	}

	/// <summary>
	/// Coroutine, playing when the ball is around to be hit by human
	/// </summary>
	/// <param name="playBackTargetPos">Position where ball should land when going after human hit it</param>
	/// <param name="racketHitBackPos">Position where the rocket hits the ball toward AI</param>
	/// <param name="racketAnimTime">Time of the racket hit animation</param>
	/// <returns></returns>
	private IEnumerator playBackCoroutine(Vector3 playBackTargetPos, Vector3 racketHitBackPos, float racketAnimTime)
	{
		yield return null;

		// may be using slowed down movement of the ball
		var xzDistance = (racketHitBackPos - ball.transform.position).magnitude;
		var xzTimeToHit = xzDistance / ball.speedXZ;

		yield return new WaitUntil(() => xzTimeToHit > racketAnimTime);

		// what to do further...
	}

	private bool _ballTriggeredPoint = false;

	private void QteAskHumanManager_AskUserFailed()
	{

	}

	private bool _canPlay = true;

	public void CanPlay(bool c)
	{
		_canPlay = c;
	}

	public HumanBallPoint Next()
	{
		return ballTargetRandomList.Next();
	}

	private Vector3 findPositionAcrossZlevelXZ(Vector3 origin, Vector3 direction, float zLevel)
	{
		// Ensure direction is not parallel to the plane
		if (Mathf.Approximately(direction.z, 0))
		{
			throw new System.ArgumentException("Direction vector is parallel to the plane, no intersection possible.");
		}

		// Calculate t
		float t = (zLevel - origin.z) / direction.z;

		// Find the intersection point
		Vector3 intersection = origin + t * direction;

		return new Vector3(intersection.x, 0, intersection.z);
	}

	private Vector3 kickDirection(Vector3 dir, float yBaseRotation, float yRandomMin, float yRandomMax)
	{
		// Step 1: Rotate around Y-axis by 15 degrees
		Quaternion yBaseRotationQuat = Quaternion.Euler(0, yBaseRotation, 0);
		Vector3 rotatedDir = yBaseRotationQuat * dir;

		// Step 2: Add random rotation around Y-axis (-5 to +5 degrees)
		float randomYRotation = Random.Range(yRandomMin, yRandomMax);
		Quaternion randomYRotationQuat = Quaternion.Euler(0, randomYRotation, 0);
		rotatedDir = randomYRotationQuat * rotatedDir;

		return rotatedDir.normalized;  // Return normalized direction vector
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ball"))
		{
			var b = other.gameObject.GetComponent<TennisBall>();

			if(b.whoKicked == KickedBy.ai)
				_ballTriggeredPoint = true;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;

		foreach (HumanBallPoint t in ballTargets)
		{
			Gizmos.DrawSphere(t.transform.position, 0.1f);
		}

		Gizmos.color = Color.green;

		//Gizmos.DrawSphere(kickingPoint.position, .1f);
	}
}
