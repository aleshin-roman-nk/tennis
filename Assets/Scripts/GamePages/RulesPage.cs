using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.UI;

public class RulesPage : MonoBehaviour
{
	[SerializeField] private GameObject rulePageElements;
	[SerializeField] private TextMeshProUGUI msgText;

	[SerializeField] private TapGesture StartingButton;

	public event EventHandler StartGame;

	// Start is called before the first frame update
	void Start()
	{
		StartingButton.Tapped += StartingButton_Tapped;
	}

	private void StartingButton_Tapped(object sender, EventArgs e)
	{
		StartGame?.Invoke(sender, EventArgs.Empty);
	}

	public void EnterPage(string msg)
	{
		msgText.text = msg;

		rulePageElements.SetActive(true);
	}

	public void ExitPage()
	{
		rulePageElements.SetActive(false);
	}
}
