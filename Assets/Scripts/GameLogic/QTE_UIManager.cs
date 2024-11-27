using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.UIElements;

public class QTE_UIManager : MonoBehaviour
{
	// If there are a lot of buttons with the same images, I need to use pool and instantiating

	[SerializeField] private RectTransform upButton;
	[SerializeField] private RectTransform downButton;
	[SerializeField] private RectTransform leftButton;
	[SerializeField] private RectTransform rightButton;
	[SerializeField] private RectTransform center;

	[SerializeField] private RectTransform activeServePrefab;

	private RectTransform parentPanel;

	Dictionary<string, RectTransform> _buttonMap = new Dictionary<string, RectTransform>();

	public event Action<string, HumanBallPoint> buttonPressed;

	private List<RectTransform> activaServeButtons = new List<RectTransform>();

	// Start is called before the first frame update
	void Start()
	{
		_buttonMap.Add("right", rightButton);
		_buttonMap.Add("left", leftButton);
		_buttonMap.Add("up", upButton);
		_buttonMap.Add("down", downButton);

		rightButton.GetComponent<PressGesture>().Pressed += QTEManager_PressedRight;
		leftButton.GetComponent<PressGesture>().Pressed += QTEManager_PressedLeft;
		downButton.GetComponent<PressGesture>().Pressed += QTEManager_PressedDown;
		upButton.GetComponent<PressGesture>().Pressed += QTEManager_PressedUp;

		parentPanel = transform as RectTransform;
	}

	private void QTEManager_PressedUp(object sender, EventArgs e)
	{
		HideBallPoint();
		buttonPressed?.Invoke("up", _current);
	}

	private void QTEManager_PressedDown(object sender, EventArgs e)
	{
		HideBallPoint();
		buttonPressed?.Invoke("down", _current);
	}

	private void QTEManager_PressedLeft(object sender, EventArgs e)
	{
		HideBallPoint();
		buttonPressed?.Invoke("left", _current);
	}

	private void QTEManager_PressedRight(object sender, System.EventArgs e)
	{
		HideBallPoint();
		buttonPressed?.Invoke("right", _current);
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private HumanBallPoint _current;

	public void ShowUserServingButtons(HumanBallPoint[] points)
	{
		if(activaServeButtons.Count == 0)
		{

		}

		int i = 0;
		foreach (HumanBallPoint point in points)
		{

		}
	}

	public void ShowCurrentBallPoint(HumanBallPoint p)
	{
		_current = p;

		Vector3 screenPosition = Camera.main.WorldToScreenPoint(_current.transform.position);

		// Convert the screen position to a local position on the Canvas
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			parentPanel,
			screenPosition,
			Camera.main,
			out Vector2 localPosition
		);

		foreach (var t in _current.getAllowedButtons)
		{
			var b = _buttonMap[t];

			var rp = b.GetComponent<QTEButtonRelativePosition>();

			b.localPosition = localPosition + 75f * rp.relPos;

			b.gameObject.SetActive(true);
		}

		center.localPosition = localPosition;
		center.gameObject.SetActive(true);
	}

	public void HideBallPoint()
	{
		foreach(var b in _buttonMap)
		{
			b.Value.gameObject.SetActive(false);
		}
		center.gameObject.SetActive(false);
	}
}
