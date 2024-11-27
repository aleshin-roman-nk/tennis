using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEAskHumanManager : MonoBehaviour
{
	[SerializeField] private QTE_UIManager qTE_UIManager;
	[SerializeField] private float ballSlowdownWhenAskingUser = 0.05f;

	public event Action AskUserStarted;
	public event Action AskUserFailed;
	public event Action<string, HumanBallPoint> UserMadeSolution;

	private bool _userMadeSolution = false;
	private TennisBall _ballUnderAsking = null;

	// Start is called before the first frame update
	void Start()
	{
		qTE_UIManager.buttonPressed += QteManager_buttonPressed;
	}

	public void ShowHumanServe(HumanBallPoint[] servPoints)
	{

	}

	private void QteManager_buttonPressed(string directionName, HumanBallPoint point)
	{
		if (_ballUnderAsking != null) _ballUnderAsking.RestoreMovement();
		//qTEManager.HideBallPoint();// comment to be sure it is only point to cancel asking buttons
		_userMadeSolution = true;
		UserMadeSolution?.Invoke(directionName, point);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ball"))
		{
			var o = other.GetComponent<TennisBall>();

			if (o.whoKicked == KickedBy.ai)
			{
				_userMadeSolution = false;
				_ballUnderAsking = o;
				o.SlowdownMovement(ballSlowdownWhenAskingUser);
				qTE_UIManager.ShowCurrentBallPoint(o.humanBallPoint);
				AskUserStarted?.Invoke();
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Ball"))
		{
			var o = other.GetComponent<TennisBall>();
			if (o.whoKicked == KickedBy.ai)
			{
				_ballUnderAsking = null;
				o.RestoreMovement();

				qTE_UIManager.HideBallPoint();

				if(!_userMadeSolution)
					AskUserFailed?.Invoke();
			}
		}
	}
}
