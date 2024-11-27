using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisBallSpawner : MonoBehaviour
{
	[SerializeField] private TennisBall tennisBallPrefab;
	//[SerializeField] private RectTransform iconsContainer;

	private PoolMono<TennisBall> ballsPool;
	// Start is called before the first frame update
	void Start()
	{
		ballsPool = new PoolMono<TennisBall>(tennisBallPrefab, 10);
		ballsPool.autoExpand = true;
	}

	public TennisBall GetBall()
	{
		return ballsPool.GetFreeElement();
	}
}
