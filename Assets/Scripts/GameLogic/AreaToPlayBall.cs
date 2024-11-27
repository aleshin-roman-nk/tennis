using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaToPlayBall : MonoBehaviour
{
	[SerializeField] private Vector3 areaToPlay = new Vector3(1, 1, 1);

	public Vector3 area => areaToPlay;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;

		Gizmos.DrawWireCube(transform.position, areaToPlay);
	}
}
