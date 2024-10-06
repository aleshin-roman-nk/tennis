using Assets.Scripts.EventBusLogic;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;

public class SimpleThinker : MonoBehaviour, IEventBusParticipant
{
	private PressGesture pressGesture;
	private EventBus _eventBus;

	// Start is called before the first frame update
	void Start()
	{
		pressGesture = GetComponent<PressGesture>();
		pressGesture.Pressed += PressGesture_Pressed;
	}

	private void PressGesture_Pressed(object sender, System.EventArgs e)
	{
		var mAmount = Random.Range(1, 10);
		_eventBus.TriggerMoreApplesFound(mAmount);
	}

	public void Bind(EventBus eventBus)
	{
		_eventBus = eventBus;
	}
}
