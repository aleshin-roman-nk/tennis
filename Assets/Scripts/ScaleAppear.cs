using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAppear : MonoBehaviour
{
	private float animationSpeed = 1.0f;

	private AnimateVector3 animateScale;

	private Vector3 originalScale;

	// Start is called before the first frame update
	void Start()
	{
		animateScale = new AnimateVector3
		{
			duration = 1.0f / animationSpeed,
			OnAnimationStep = (x) =>
			{
				transform.localScale = x;
			}
		};
	}

	public void SetAnimationSpeed(float s)
	{
		animationSpeed = s;
		animateScale.duration = 1.0f / animationSpeed;
	}

	public void Appear(bool a)
	{
		if (a) StartCoroutine(appear());
		else StartCoroutine(disappear());
	}

	private IEnumerator appear()
	{
		animateScale.valueA = Vector3.zero;
		animateScale.valueB = originalScale;

		while (animateScale.update(null))
		{
			yield return null;
		}
	}

	private bool scaleSaved = false;
	private IEnumerator disappear()
	{
		if (!scaleSaved) { originalScale = transform.localScale; scaleSaved = true; }

		animateScale.valueA = originalScale;
		animateScale.valueB = Vector3.zero;

		while (animateScale.update(null))
		{
			yield return null;
		}
	}
}
