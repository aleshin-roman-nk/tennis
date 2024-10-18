using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;

public class TouchObserver : MonoBehaviour
{
	[SerializeField] private TennisMan tennisMan;
	[SerializeField] private TennisBall tennisBallPrefab;
	[SerializeField] private Transform kickingPoint;
	[SerializeField] private float kickingForce = 10f;

	private PressGesture pressGesture;

	private PoolMono<TennisBall> ballsPool;

	// Start is called before the first frame update
	void Start()
	{
		pressGesture = GetComponent<PressGesture>();
		pressGesture.Pressed += PressGesture_Pressed;

		ballsPool = new PoolMono<TennisBall>(tennisBallPrefab, 10);
		ballsPool.autoExpand = true;
	}

	private void PressGesture_Pressed(object sender, System.EventArgs e)
	{
		Vector3 touchPosition = pressGesture.ScreenPosition;

		touchPosition.z = 1f;

		touchPosition = Camera.main.ScreenToWorldPoint(touchPosition);

		var ball = ballsPool.GetFreeElement();
		ball.transform.position = touchPosition;

		var kickDirection = (touchPosition - kickingPoint.position).normalized;

		ball.Kick(kickingForce * kickDirection);

		tennisMan.PassBall(ball);
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		Gizmos.DrawSphere(kickingPoint.position, .1f);
	}
}
