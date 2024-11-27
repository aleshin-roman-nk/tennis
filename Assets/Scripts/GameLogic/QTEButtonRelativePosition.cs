using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEButtonRelativePosition : MonoBehaviour
{
	[SerializeField] float X = 0;
	[SerializeField] float Y = 0;

	public Vector2 relPos => new Vector2(X, Y);
}
