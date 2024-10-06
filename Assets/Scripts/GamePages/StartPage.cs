using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.UI;

public class StartPage : MonoBehaviour
{
	[SerializeField] private GameObject startPageElementsGroup;
	[SerializeField] private Image startPageBackground;
	[SerializeField] private TapGesture StartingButton;

	public event EventHandler StartGame;

	// Start is called before the first frame update
	void Start()
	{
		StartingButton.Tapped += StartingButton_Tapped;
	}

	private void StartingButton_Tapped(object sender, System.EventArgs e)
	{
		StartGame?.Invoke(this, EventArgs.Empty);
	}

	public void EnterPage()
	{
		startPageElementsGroup.SetActive(true);
		startPageBackground.gameObject.SetActive(true);
	}

	public void ExitPage()
	{
		startPageElementsGroup.SetActive(false);
		startPageBackground.gameObject.SetActive(false);
	}
}
