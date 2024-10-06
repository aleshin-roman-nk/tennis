using Assets.Scripts.EventBusLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	public class EventBusManager: MonoBehaviour
	{
		[SerializeField] private MonoBehaviour[] participants;

		EventBus bus = new EventBus();

		private void Start()
		{
			foreach (var participant in participants)
			{
				if (participant is IEventBusParticipant eventParticipant)
				{
					eventParticipant.Bind(bus);
				}
			}
		}
	}
}
