using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBus
{
	public UnityEvent<int> MoreApplesFound { get; } = new();
	public void TriggerMoreApplesFound(int amount)
	{
		MoreApplesFound.Invoke(amount);
	}
}
