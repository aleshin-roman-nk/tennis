using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;

public class HitButtonManager : MonoBehaviour
{
	[SerializeField] private PressGesture leftButton;
	[SerializeField] private PressGesture upButton;
	[SerializeField] private PressGesture rightButton;

	private void Start()
	{
		leftButton.Pressed += LeftButton_Pressed;
		upButton.Pressed += UpButton_Pressed;
		rightButton.Pressed += RightButton_Pressed;
	}

	private void RightButton_Pressed(object sender, System.EventArgs e)
	{
		Debug.Log("RightButton_Pressed");
	}

	private void UpButton_Pressed(object sender, System.EventArgs e)
	{
		Debug.Log("UpButton_Pressed");
	}

	private void LeftButton_Pressed(object sender, System.EventArgs e)
	{
		Debug.Log("LeftButton_Pressed");
	}
}
