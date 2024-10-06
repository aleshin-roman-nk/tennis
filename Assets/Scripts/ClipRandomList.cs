using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipRandomList : MonoBehaviour
{
	[SerializeField] private AudioClip[] clips;

	RandomList<AudioClip> randomList;

	public AudioClip Next()
	{
		if (randomList == null)
			randomList = new RandomList<AudioClip>(clips);

		return randomList.Next();
	}
}
