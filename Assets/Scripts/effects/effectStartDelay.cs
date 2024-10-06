using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectStartDelay : MonoBehaviour
{
	[SerializeField] private GameObject effect;
	[SerializeField] private float delay;

	[SerializeField] private Canvas canvas;

	// Start is called before the first frame update
	void Start()
	{

	}

	private void OnEnable()
	{
		StartCoroutine(go());
	}

	private IEnumerator go()
	{
		yield return new WaitForSeconds(delay);

		//Instantiate(effect, transform.position, Quaternion.identity).layer = this.gameObject.layer;
		Instantiate(effect, transform);
	}
}
