using System.Collections;
using UnityEngine;

public class CrowdAnimationController : MonoBehaviour
{
	[SerializeField] private int _animationCount = 3;
	[SerializeField] private float _minSpeed = 0.8f;
	[SerializeField] private float _maxSpeed = 1.2f;
	[SerializeField] private int _waitingStateIndex = -1;

	private Animator _animator;
	private AIPlayer _goalChecker;
	private Coroutine _animationCoroutine;
	private int _currentAnimationIndex = -1;

	private void OnEnable() => _goalChecker.OnGoal += OnGoal;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_goalChecker = FindFirstObjectByType<AIPlayer>();
	}

	private void Start() => InitializeRandomAnimation();

	private void OnDisable() => _goalChecker.OnGoal -= OnGoal;

	private void InitializeRandomAnimation() => ChangeAnimation();

	private void OnGoal()
	{
		_animator.SetTrigger("Goal");

		if (_animationCoroutine != null)
		{
			StopCoroutine(_animationCoroutine);
		}

		_animationCoroutine = StartCoroutine(ChangeAnimationWithDelay());
	}

	private IEnumerator ChangeAnimationWithDelay()
	{
		ChangeAnimation();

		float delay = Random.Range(3f, 6f);
		yield return new WaitForSeconds(delay);

		ChangeAnimation();
	}

	private void ChangeAnimation()
	{
		_animator.SetInteger("AnimationIndex", _waitingStateIndex);

		int newAnimationIndex;
		do
		{
			newAnimationIndex = Random.Range(0, _animationCount);
		} while (newAnimationIndex == _currentAnimationIndex);

		_currentAnimationIndex = newAnimationIndex;

		_animator.SetInteger("AnimationIndex", _currentAnimationIndex);
		_animator.speed = Random.Range(_minSpeed, _maxSpeed);
	}
}
