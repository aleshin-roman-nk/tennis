using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisMan : MonoBehaviour
{
	[SerializeField] private Animator[] animators;

	[SerializeField] private AreaToPlayBall areaToPlayBall;

	[SerializeField] private TennisManBallStriker rightStrike;
	[SerializeField] private TennisManBallStriker leftStrike;

	private Queue<TennisBall> tennisBalls = new Queue<TennisBall>();
	private TennisBall ballCurrentObservable = null;

	private LinearMoveXZStep moveToHitBallPoint = new LinearMoveXZStep();


	private MoveObjectWithAcceleration moveObjectWithAcceleration = new MoveObjectWithAcceleration();

	public void AddBall(TennisBall tennisBall)
	{
		tennisBalls.Enqueue(tennisBall);
	}

	// Start is called before the first frame update
	void Start()
	{
		moveToHitBallPoint.adjustDirection = false;
		moveToHitBallPoint.movableObject = transform;
		moveToHitBallPoint.speed = 6;

		moveObjectWithAcceleration.movableObject = transform;

		//foreach (var animator in animators)
		//{
		//	AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
		//	foreach (AnimationClip clip in clips)
		//	{
		//		Debug.Log($"Animation {clip.name} Length: {clip.length} seconds");
		//	}
		//}
	}

	// Update is called once per frame
	void Update()
	{
		//if (ballCurrentObservable == null && tennisBalls.Count > 0)
		//	startObservingBall(tennisBalls.Dequeue());
		//else if(ballCurrentObservable != null)
		//{

		//}
	}

	public void PassBall(TennisBall b)
	{
		//StartCoroutine(playBallCoroutine(b));
		StartCoroutine(playBallCoroutineAccelerate(b));
	}

	private IEnumerator playBallCoroutineAccelerate(TennisBall b)
	{
		ballCurrentObservable = b;

		yield return new WaitUntil(() => b.movmentDefined);

		var ballFallPoint = findPointWhereBallFallsXZ(ballCurrentObservable, areaToPlayBall);

		var rightSide = true;
		Vector3 tennisManPositionToStrikeBall = Vector3.zero;

		if (Vector3.Distance(ballFallPoint, rightStrike.transform.position) < Vector3.Distance(ballFallPoint, leftStrike.transform.position))
		{
			rightSide = true;
			tennisManPositionToStrikeBall = transform.position + (ballFallPoint - rightStrike.transform.position);// + ballCurrentObservable.speed * ballCurrentObservable.direction * 0.5f;
		}
		else
		{
			rightSide = false;
			tennisManPositionToStrikeBall = transform.position + (ballFallPoint - leftStrike.transform.position);// + ballCurrentObservable.speed * ballCurrentObservable.direction * 0.5f;
		}

		moveObjectWithAcceleration.OnStartMoving = () =>
		{
			if (rightSide)
				foreach (var animator in animators)
				{
					animator.SetTrigger("Left");
				}
			else
			{
				foreach (var animator in animators)
				{
					animator.SetTrigger("Right");
				}
			}
		};

		yield return StartCoroutine(moveObjectWithAcceleration.update(transform.position, tennisManPositionToStrikeBall, 0, 6, 0.5f));

		idle();

		const float hitAnimationTime = 0.6f;
		var distanceHit = ballCurrentObservable.speed * hitAnimationTime;

		var tennisManPositionToStrikeBallXZ = new Vector3(tennisManPositionToStrikeBall.x, 0, tennisManPositionToStrikeBall.z);
		var ballPositionCurrentXZ = new Vector3(ballCurrentObservable.transform.position.x, 0, ballCurrentObservable.transform.position.z);

		// add condition: if the rest of ball path is less than hitting animation lenght, the man is lost and does not start hitting
		if(Vector3.Distance(ballPositionCurrentXZ, tennisManPositionToStrikeBallXZ) < distanceHit)
		{
			yield break;
		}

		yield return new WaitUntil(() => 
		{
			ballPositionCurrentXZ = new Vector3(ballCurrentObservable.transform.position.x, 0, ballCurrentObservable.transform.position.z);
			return Vector3.Distance(ballPositionCurrentXZ, tennisManPositionToStrikeBallXZ) <= distanceHit;
		});

		yield return StartCoroutine(hitBall(rightSide));

		idle();
	}

	private void idle()
	{
		foreach (var animator in animators)
		{
			animator.SetTrigger("Idle");
		}
	}

	private IEnumerator hitBall(bool rightSide)
	{
		if (rightSide)
		{
			foreach (var animator in animators)
			{
				animator.SetTrigger("Backhand");
			}
		}
		else
		{
			foreach (var animator in animators)
			{
				animator.SetTrigger("Forehand");
			}
		}

		yield return new WaitForSeconds(0.66f);

		//Debug.Log("hitBall>");

		//string animationTrigger = rightSide ? "Backhand" : "Forehand";

		//// Set the appropriate animation trigger
		//foreach (var animator in animators)
		//{
		//	animator.SetTrigger(animationTrigger);
		//}

		//// Wait until the animation is finished
		//foreach (var animator in animators)
		//{
		//	yield return new WaitUntil(() =>
		//	{
		//		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0 is the layer index
		//		//Debug.Log(stateInfo.length);
		//		return stateInfo.IsName(animationTrigger) && stateInfo.normalizedTime >= 1;
		//	});
		//}

		//Debug.Log("hitBall<");
	}

	private Vector3 findPointWhereBallFallsXZ(TennisBall b, AreaToPlayBall a)
	{
		Vector3 pos = b.transform.position;  // Current position of the object (x1, z1)
		Vector3 dir = b.direction;   // Assuming direction is normalized (dx, dz)

		var z1 = a.transform.position.z - a.AreaToPlay.z / 2;
		var z2 = a.transform.position.z + a.AreaToPlay.z / 2;

		// Calculate t-values for z1 = -5 and z2 = -10
		float t1 = (z1 - pos.z) / dir.z;
		float t2 = (z2 - pos.z) / dir.z;

		// Get a random t-value between t1 and t2
		float tRandom = Random.Range(t1, t2);

		// Calculate the random x and z coordinates at the random t-value
		float xRandom = pos.x + dir.x * tRandom;
		float zRandom = pos.z + dir.z * tRandom;

		// The random point between z = -5 and z = -10 on the trajectory
		//Vector3 randomPoint = new Vector3(xRandom, pos.y, zRandom);

		return new Vector3(xRandom, 0, zRandom);
	}
}
