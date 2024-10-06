using Assets.Scripts.EventBusLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
	[SerializeField] private GameScoresManager gameScoresManager;

	EventBus eventBus;
	public event EventHandler GameOver;

	private AudioSource audioSource;

	private bool gameIsAlreadyOver = false;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void CdTimer_Expired(object sender, EventArgs e)
	{
		if(!gameIsAlreadyOver) GameOver?.Invoke(this, EventArgs.Empty);
	}

	public void PlayGame()
	{
		gameIsAlreadyOver = false;
		gameScoresManager.ResetScores();
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
}
