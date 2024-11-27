using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;

public class HumanBallPoint : MonoBehaviour
{
	[SerializeField] private PressGesture userServeButtonImage;
	[SerializeField] private string[] _alloweeDirectionButtons = new string[] { "right", "up", "left", "down" };
	[SerializeField] private string _levelName;
	public string[] getAllowedButtons => _alloweeDirectionButtons;

	public string levelName => _levelName;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
