using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;

public class CountUpTimer : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI timeText;

	public int Seconds = 0;

	private bool stopped = false;
	private bool paused = false;

	public void Go()
	{
		stopped = false;
		paused = false;
		StartCoroutine(timerCoroutine());
	}

	public void StopTimer()
	{
		stopped = true;
	}

	public void PauseTimer()
	{
		paused = true;
	}

	public void ResumeTimer()
	{
		paused = false;
	}

	private IEnumerator timerCoroutine()
	{
		Seconds = 0;

		while (!stopped)
		{
			timeText.text = Seconds.ToString("0");
			yield return new WaitForSeconds(1f);
			if (!paused) Seconds++;
		}

		timeText.text = Seconds.ToString();

		timeText.text = 0.ToString();
	}
}
