using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;

public class GamePage : MonoBehaviour
{
	[SerializeField] private GameObject gamePageElementsGroup;

	// Start is called before the first frame update
	void Start()
	{

	}

	public void EnterPage()
	{
		gamePageElementsGroup.SetActive(true);
	}

	public void ExitPage()
	{
		gamePageElementsGroup.SetActive(false);
	}
}
