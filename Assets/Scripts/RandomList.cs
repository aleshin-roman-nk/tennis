using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomList<T>
{
	private List<T> list = new List<T>();

	private int _lastNumber = -1;
	private int _secondLastNumber = -1;

	private readonly bool _canRepeatTwice;

	public RandomList(T[] clips, bool canRepeatTwice = true)
	{
		_canRepeatTwice = canRepeatTwice;
		list.Clear();
		list.AddRange(clips);
	}

	public T Next()
	{
		if (list.Count == 0) return default(T);

		if(list.Count == 1) return list[0];

		int nextNumber;

		if (_canRepeatTwice)
		{
			do
			{
				nextNumber = Random.Range(0, list.Count);
			} while (nextNumber == _lastNumber && nextNumber == _secondLastNumber);
		}
		else
		{
			do
			{
				nextNumber = Random.Range(0, list.Count);
			} while (nextNumber == _lastNumber);
		}


		_secondLastNumber = _lastNumber;
		_lastNumber = nextNumber;

		return list[nextNumber];
	}
}
