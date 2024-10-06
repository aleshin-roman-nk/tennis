using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.UI;

public class FinalPage : MonoBehaviour
{
	[SerializeField] private GameObject finalPageElementsGroup;
	[SerializeField] private Image finalPageBackground;

	[SerializeField] private TextMeshProUGUI CountdownRestartText;
	[SerializeField] private TextMeshProUGUI greatingText;
	[SerializeField] private TapGesture RestartingButton;

	private bool waitForRestart = false;

	public event EventHandler RestartClicked;

	private AnimateFloat animateBg;

	private void Start()
	{
		RestartingButton.Tapped += RestartingButton_Tapped;

		animateBg = new AnimateFloat
		{
			duration = 0.7f,
			OnAnimationStep = (a) =>
			{
				Color newColor = finalPageBackground.color;
				newColor.a = a;
				finalPageBackground.color = newColor;
			}
		};
	}

	private void RestartingButton_Tapped(object sender, System.EventArgs e)
	{
		RestartClicked?.Invoke(this, EventArgs.Empty);
	}

	private IEnumerator countDownToRestart()
	{
		waitForRestart = true;

		int remainingTime = 9;

		while (remainingTime > 0)
		{
			CountdownRestartText.text = $"RESTART IN {remainingTime}...";
			yield return new WaitForSeconds(1f);
			remainingTime--;
		}

		CountdownRestartText.gameObject.SetActive(false);
		RestartingButton.gameObject.SetActive(true);

		waitForRestart = false;
	}

	public void GreatingText(string msg)
	{
		greatingText.text = msg;
	}

	private IEnumerator entering()
	{
		finalPageBackground.gameObject.SetActive(true);
		RestartingButton.gameObject.SetActive(false);

		animateBg.valueA = 0f;
		animateBg.valueB = 0.9f;

		while (animateBg.update(null)) yield return null;

		finalPageElementsGroup.SetActive(true);
		CountdownRestartText.gameObject.SetActive(true);

		StartCoroutine(countDownToRestart());
	}

	public void EnterPage()
	{
		StartCoroutine(entering());
	}

	public void ExitPage()
	{
		finalPageElementsGroup.SetActive(false);
		finalPageBackground.gameObject.SetActive(false);
		RestartingButton.gameObject.SetActive(false);
		Color newColor = finalPageBackground.color;
		newColor.a = 0.0f;
		finalPageBackground.color = newColor;
	}
}
