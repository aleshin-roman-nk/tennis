using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
	void PlayGame();
	void PauseGame();
	void ResumeGame();
	void StopGame();
	event Action GameOver;
}
