using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
	[SerializeField] private GameMusicManager gameMusicManager;

	[SerializeField] private TennisBall ball;

	[SerializeField] private Animator[] animators;

	[SerializeField] private AreaToPlayBall globalAreaToPlayBall;
	[SerializeField] private AreaToPlayBall localAreaOfMan;

	[SerializeField] private TennisManBallStriker rightStrike;
	[SerializeField] private TennisManBallStriker leftStrike;

	[SerializeField] private float manSpeedMin = 3.0f;
	[SerializeField] private float manSpeedMax = 10.0f;
	[SerializeField] private float ballHitTime = 0.66f;

	private AudioSource _player;

	private MoveWithAccelerationInStart moveWithAccelerationInStart = new MoveWithAccelerationInStart();

	public event Action OnGoal;

	// Start is called before the first frame update
	void Start()
	{
		rightStrike.gameObject.SetActive(false);
		leftStrike.gameObject.SetActive(false);
		moveWithAccelerationInStart.movableObject = transform;

		_player = GetComponent<AudioSource>();
	}

	public void ServeBall()
	{

	}

	public void PlayBall(TennisBall b)
	{
		StartCoroutine(playBallCoroutineAccelerate(b));
	}

	private IEnumerator playBallCoroutineAccelerate(TennisBall b)
	{
		var ballStartPosXZ = new Vector3(ball.transform.position.x, 0, ball.transform.position.z);

		Vector3 ballMeetPosXZ;

		if (!hitPointInArea(ball, globalAreaToPlayBall, localAreaOfMan, out ballMeetPosXZ))
		{
			yield break;
		}

		var ballDistance = Vector3.Distance(ballMeetPosXZ, ballStartPosXZ);
		var ballMeetTime = ballDistance / ball.speedXZ;

		var leftSide = true;
		string side = string.Empty;
		Vector3 tennisManTargetPoint = Vector3.zero;

		// if hit point is at right or lift sector
		if (Vector3.Distance(ballMeetPosXZ, rightStrike.transform.position) < Vector3.Distance(ballMeetPosXZ, leftStrike.transform.position))
		{
			leftSide = true;
			tennisManTargetPoint = transform.position + (ballMeetPosXZ - rightStrike.transform.position);

			side = DetermineSector(rightStrike.transform.position, ballMeetPosXZ, angleA);
		}
		else
		{
			leftSide = false;
			tennisManTargetPoint = transform.position + (ballMeetPosXZ - leftStrike.transform.position);

			side = DetermineSector(leftStrike.transform.position, ballMeetPosXZ, angleA);
		}

		var tennisManTimeToMeet = ballMeetTime - ballHitTime;
		var tennisManDistanceToMeet = Vector3.Distance(tennisManTargetPoint, transform.position);
		var tennisManVelocity = tennisManDistanceToMeet / tennisManTimeToMeet;

		if (tennisManVelocity > manSpeedMax)
		{
			tennisManVelocity = manSpeedMax;
		} else if(tennisManVelocity <= manSpeedMin)
		{
			float tMin = tennisManDistanceToMeet / manSpeedMin;
			float tWaiting = tennisManTimeToMeet - tMin;
			yield return new WaitForSeconds(tWaiting);
			tennisManVelocity = manSpeedMin;
		}

		var fallChance = UnityEngine.Random.Range(0, 100);
		if (fallChance >= 80)
		{
			tennisManVelocity *= 0.8f;
		}

		moveWithAccelerationInStart.OnStartMoving = () =>
		{
			moveAnimation(side, tennisManVelocity);
		};
		moveWithAccelerationInStart.StopCondition = () =>
		{
			return ball.hitNet;
		};

		var movingCoroutine = StartCoroutine(moveWithAccelerationInStart.update(
			transform.position,
			tennisManTargetPoint,
			0,
			tennisManVelocity,
			0.5f));

		yield return movingCoroutine;

		//Debug.Log($"before moving {ballCurrent.speedXZ * ballCurrent.directionXZ}");
		
		if (ball.hitNet)
		{
			//forgetBall();
			Idle();
			yield break;
		}

		// check the rest of time the ball to the hit point
		// if this is worth to start hit
		// t = s / v
		float ballRestDistanceToHit = Vector3.Distance(ballMeetPosXZ, positionXZ(ball.transform.position));
		float ballRestTimeToHit = ballRestDistanceToHit / ball.speedXZ;

		if(ballRestTimeToHit < 0.30f || (transform.position.z - ball.transform.position.z > 0))
		{
			Idle();
			playGoal();
			yield break;
		}

		if (!leftSide) leftStrike.gameObject.SetActive(true); else rightStrike.gameObject.SetActive(true);

		yield return StartCoroutine(hitBall(leftSide, ballHitTime));

		if (!leftSide) leftStrike.gameObject.SetActive(false); else rightStrike.gameObject.SetActive(false);

		Idle();

		// sometimes need delay to detect goal for sure
		yield return new WaitForSeconds(0.5f);

		var zzz = transform.position.z - ball.transform.position.z;

		if (zzz > 0) playGoal();
	}

	private Vector3 positionXZ(Vector3 p)
	{
		return new Vector3(p.x, 0, p.z);
	}

	private void playGoal()
	{
		OnGoal?.Invoke();
	}

	private void SetAnimationSpeed(float speed)
	{
		foreach (var animator in animators)
		{
			animator.speed = speed;
		}
	}

	private void Idle()
	{
		foreach (var animator in animators)
		{
			animator.speed = 1;
			animator.SetTrigger("Idle");
		}
	}

	private void Backhand()
	{
		foreach (var animator in animators)
		{
			animator.speed = 1;
			animator.SetTrigger("Backhand");
		};
	}

	private void Forehand()
	{
		foreach (var animator in animators)
		{
			animator.speed = 1;
			animator.SetTrigger("Forehand");
		};
	}

	private void moveAnimation(string sideName, float speed)
	{
		var animName = string.Empty;

		/*
		if (thetaP <= halfA || thetaP >= 360 - halfA)
			return "Right";
		if (thetaP >= 180 - halfA && thetaP <= 180 + halfA)
			return "Left";
		if (thetaP >= 90 - halfA && thetaP <= 90 + halfA)
			return "Front";
		if (thetaP >= 270 - halfA && thetaP <= 270 + halfA)
			return "Behind";
		 */

		// Normalize speed between 0 and 1
		float normalizedSpeed = Mathf.InverseLerp(0, manSpeedMax, speed);

		// Adjust the animation speed
		var animSpeed = Mathf.Lerp(0.4f, 1.5f, normalizedSpeed);

		if (sideName.Equals("Right")) animName = "Right";
		else if (sideName.Equals("Left")) animName = "Left";
		else if (sideName.Equals("Front")) animName = "Forward";
		else if (sideName.Equals("Behind")) animName = "Backward";

		foreach (var animator in animators)
		{
			animator.speed = animSpeed;
			animator.SetTrigger(animName);
		};
	}

	private IEnumerator hitBall(bool rightSide, float hitTime)
	{
		if (rightSide) Backhand();
		else Forehand();

		yield return new WaitForSeconds(hitTime);
	}

	private bool hitPointInArea(TennisBall b, AreaToPlayBall globalarea, AreaToPlayBall localArea, out Vector3 p)
	{
		Vector3 ballPos = b.transform.position;  // Current position of the object (x1, z1)
		Vector3 ballDir = b.directionXZ;   // Assuming direction is normalized (dx, dz)

		var zBackLocal = localArea.transform.position.z - localArea.area.z / 2;
		var zFrontLocal = localArea.transform.position.z + localArea.area.z / 2;

		var zBackGlobal = globalarea.transform.position.z - globalarea.area.z / 2;
		var zFrontGlobal = globalarea.transform.position.z + globalarea.area.z / 2;

		//===========get x-axis limits (limits of sides)===============
		// now, we need to define if ball trajectory intersects man's local area
		//float t = (z2 - ballPos.z) / ballDir.z;

		// why not to get point just between z1 and z2 of tennis man's position?
		//float x1 = transform.position.x - a.area.x / 2;
		//float x2 = transform.position.x + a.area.x / 2;

		//float X = ballPos.x + ballDir.x * t;

		//if (!(X >= x1 && X <= x2))
		//{
		//	p = Vector3.zero;
		//	return false;
		//}
		//==========================

		float t1 = 0;
		float t2 = 0;

		// Calculate t-values for z1 and z2
		if (zBackLocal < zBackGlobal)
		{

			t1 = (zBackGlobal - ballPos.z) / ballDir.z;
			t2 = (zFrontLocal - ballPos.z) / ballDir.z;
		}
		else if (zFrontLocal > zFrontGlobal)
		{
			t1 = (zBackLocal - ballPos.z) / ballDir.z;
			t2 = (zFrontGlobal - ballPos.z) / ballDir.z;
		}
		else
		{
			t1 = (zBackLocal - ballPos.z) / ballDir.z;
			t2 = (zFrontLocal - ballPos.z) / ballDir.z;
		}

		//// Calculate t-values for z1 and z2
		//float t1 = (zBackLocal - ballPos.z) / ballDir.z;
		//float t2 = (zFrontLocal - ballPos.z) / ballDir.z;

		// Get a random t-value between t1 and t2
		float tRandom = UnityEngine.Random.Range(t1, t2);

		// Calculate the random x and z coordinates at the random t-value
		float xRandom = ballPos.x + ballDir.x * tRandom;
		float zRandom = ballPos.z + ballDir.z * tRandom;

		p = new Vector3(xRandom, 0, zRandom);

		return true;
	}

	string DetermineSector(Vector3 origin, Vector3 point, float angleA)
	{
		// Calculate angle of point P from the origin
		float thetaP = Mathf.Atan2(point.z - origin.z, point.x - origin.x) * Mathf.Rad2Deg;
		if (thetaP < 0) thetaP += 360;

		// Define the angles for each sector
		float halfA = angleA / 2f;

		// Check each sector
		if (thetaP <= halfA || thetaP >= 360 - halfA)
			return "Right";
		if (thetaP >= 180 - halfA && thetaP <= 180 + halfA)
			return "Left";
		if (thetaP >= 90 - halfA && thetaP <= 90 + halfA)
			return "Front";
		if (thetaP >= 270 - halfA && thetaP <= 270 + halfA)
			return "Behind";

		return "Unknown";
	}

	private float angleA = 120;
	private float lineLength = 4;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, localAreaOfMan.area);

		///
		// Calculate the directions of the two lines
		float halfAngle = angleA / 2f;

		// Define directions for both lines
		Vector3 direction1 = Quaternion.Euler(0, -halfAngle, 0) * Vector3.right;
		Vector3 direction2 = Quaternion.Euler(0, halfAngle, 0) * Vector3.right;

		// Draw the lines using Gizmos
		Vector3 vpos = new Vector3(rightStrike.transform.position.x, 0.1f, rightStrike.transform.position.z);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(vpos - direction1 * lineLength, vpos + direction1 * lineLength);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(vpos - direction2 * lineLength, vpos + direction2 * lineLength);

		vpos = new Vector3(leftStrike.transform.position.x, 0.1f, leftStrike.transform.position.z);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(vpos - direction1 * lineLength, vpos + direction1 * lineLength);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(vpos - direction2 * lineLength, vpos + direction2 * lineLength);
	}

	private void playMusic(AudioClip clip)
	{
		_player.clip = clip;
		_player.Play();
	}

	private void playBallHit()
	{
		_player.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
		//_player.loop = false;
		_player.PlayOneShot(gameMusicManager.ballHitSoundList.Next());
	}
}
