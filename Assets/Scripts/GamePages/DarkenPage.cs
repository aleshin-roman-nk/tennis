using System.Collections;
using System.Collections.Generic;
using TouchScript.Examples.UI;
using UnityEngine;
using UnityEngine.UI;

public class DarkenPage : MonoBehaviour
{
	[SerializeField] private Image darkBackground;

	//public float duration;
	//public float darkPoint;

	private AnimateFloat animateDarkness;

	// Start is called before the first frame update
	void Start()
	{
		animateDarkness = new AnimateFloat
		{
			OnAnimationStep = (x) =>
			{
				Color newColor = darkBackground.color;
				newColor.a = x;
				darkBackground.color = newColor;
			}
		};
	}

	public void DarkenInstantly(float d)
	{
		darkBackground.gameObject.SetActive(true);

		Color newColor = darkBackground.color;
		newColor.a = d;
		darkBackground.color = newColor;
	}

	public IEnumerator DarkenScreen(float darkness, float duration)
	{
		if(darkness == 0)
		{
			Debug.LogWarning("goal darkness is zero, nothing to darken");
		}

		darkBackground.gameObject.SetActive(true);

		animateDarkness.duration = duration;
		animateDarkness.valueA = 0;
		animateDarkness.valueB = darkness;

		while(animateDarkness.update(null))
			yield return null;
	}

	public IEnumerator UndarkenScreen(float duration)
	{
		animateDarkness.duration = duration;
		animateDarkness.valueA = darkBackground.color.a;
		animateDarkness.valueB = 0;

		while (animateDarkness.update(null))
			yield return null;

		darkBackground.gameObject.SetActive(false);
	}
}
