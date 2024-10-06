using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMeY : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		transform.Rotate(Vector3.up, Random.Range(0.0f, 360.0f));
	}
}
