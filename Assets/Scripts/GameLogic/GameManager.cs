using Assets.Scripts.EventBusLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
	[SerializeField] GameMusicManager gameMusicManager;
	[SerializeField] private HumanPlayer humanPlayer;
	[SerializeField] private AIPlayer tennisMan;
	[SerializeField] private GameScoresManager gameScoresManager;
	[SerializeField] private int BallsCount = 10;

	public event Action GameOver;

	[SerializeField] private AudioSource crowdNoiseAudioSource;
	[SerializeField] private AudioSource crowdGoalAudioSource;

	private int goals = 0;
	private int scores = 0;

	private bool gameIsAlreadyOver = false;

	private void Start()
	{

	}

	private void OnEnable()
	{
		tennisMan.OnGoal += TennisMan_OnGoal;
	}

	private void OnDisable()
	{
		tennisMan.OnGoal -= TennisMan_OnGoal;
	}

	private void TennisMan_OnGoal()
	{
		return;

		goals++;

		if (goals == 0)
			scores = 0;
		else if (goals == 1)
			scores = 15;
		else if (goals == 2)
			scores = 30;
		else if (goals == 3)
			scores = 40;

		//playGoal();

		gameScoresManager.SetScores(scores);

		if (goals == 4)
		{
			StartCoroutine(endSession());
		}
	}

	public void PlayGame()
	{
		playCrowd();
		humanPlayer.CanPlay(true);
		goals = 0;
		scores = 0;
		gameIsAlreadyOver = false;
		gameScoresManager.ResetScores();
	}

	private IEnumerator endSession()
	{
		humanPlayer.CanPlay(false);
		yield return new WaitForSeconds(3);
		GameOver?.Invoke();
	}

	public void PauseGame()
	{
		throw new NotImplementedException();
	}

	public void ResumeGame()
	{
		throw new NotImplementedException();
	}

	public void StopGame()
	{
		throw new NotImplementedException();
	}

	private void playCrowd()
	{
		//crowdGoalAudioSource.Stop();
		//crowdNoiseAudioSource.clip = gameMusicManager.crowdSoundList.Next();
		//crowdNoiseAudioSource.loop = true;
		//crowdGoalAudioSource.Play();
	}

	private void playGoal()
	{
		crowdGoalAudioSource.Stop();
		crowdGoalAudioSource.clip = gameMusicManager.goalSoundList.Next();
		crowdNoiseAudioSource.loop = false;
		crowdGoalAudioSource.Play();
	}
}
