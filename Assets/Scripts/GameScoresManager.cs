using Assets.Scripts.EventBusLogic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameScoresManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI ScoresText;

	public int Scores {get; private set; }

	private void Start()
	{
		Scores = 0;
		ScoresText.text = Scores.ToString();
	}

	public void AddScore(int scores)
	{
		Scores += scores;

		ScoresText.text = Scores.ToString();
	}

	public void SetScores(int scores)
	{
		Scores = scores;
		ScoresText.text = Scores.ToString();
	}

	public void ResetScores()
	{
		Scores = 0;
		ScoresText.text = Scores.ToString();
	}
}
