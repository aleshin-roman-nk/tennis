using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TouchScript.Examples.Tap;
using UnityEngine;

public class ReadySetGoTimer : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI cdTextPro;

	public event EventHandler OnStart;

	public IEnumerator rsgStarting()
	{
		yield break;

		cdTextPro.color = Color.yellow;
		//cdTextPro.color = Color.white;


		//cdTextPro.text = "READY";
		cdTextPro.gameObject.SetActive(true);
		//yield return new WaitForSeconds(0.5f);

		//cdTextPro.color = new Color(1.0f, 0.5f, 0.0f);
		//cdTextPro.text = "SET";
		//yield return new WaitForSeconds(0.5f);

		//cdTextPro.color = Color.red;
		//cdTextPro.text = "GO!";
		//yield return new WaitForSeconds(0.5f);

		cdTextPro.color = Color.red;
		cdTextPro.text = 5.ToString();
		yield return new WaitForSeconds(1);

		cdTextPro.color = Color.red;
		cdTextPro.text = 4.ToString();
		yield return new WaitForSeconds(1);

		cdTextPro.color = Color.red;
		cdTextPro.text = 3.ToString();
		yield return new WaitForSeconds(1);

		cdTextPro.color = Color.red;
		cdTextPro.text = 2.ToString();
		yield return new WaitForSeconds(1);

		cdTextPro.color = Color.red;
		cdTextPro.text = 1.ToString();
		yield return new WaitForSeconds(1);

		cdTextPro.color = Color.red;
		cdTextPro.text = 0.ToString();
		yield return new WaitForSeconds(1);

		cdTextPro.gameObject.SetActive(false);

		OnStart?.Invoke(this, EventArgs.Empty);
	}
}
