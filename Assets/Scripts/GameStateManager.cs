using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Loading;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
	[SerializeField] private DarkenPage darkenPage;
	[SerializeField] private GamePage gamePage;
	[SerializeField] private FinalPage finalPage;
	[SerializeField] private RulesPage rulesPage;

	[SerializeField] private GameManager gameManager;
	[SerializeField] private ReadySetGoTimer rsgTimer;

	[SerializeField] private string[] greetingTexts;

	[SerializeField] private GameObject preventGaming;

	[SerializeField] private string ruleText;

	private IGameManager _gameManager;

	private AudioSource player;

	private RandomList<string> greetingTextsRandomList;

	// Start is called before the first frame update
	void Start()
	{
		_gameManager = gameManager;

		_gameManager.GameOver += _gameManager_GameOver;

		StartCoroutine(firstInitAndStartCoroutine());
	}

	private void _gameManager_GameOver()
	{
		StartCoroutine(enterFinalPageCoroutine());
	}

	private IEnumerator firstInitAndStartCoroutine()
	{
		yield return new WaitForEndOfFrame();

		player = GetComponent<AudioSource>();
		player.loop = true;
;
		rulesPage.StartGame += RulesPage_StartGame;
		finalPage.RestartClicked += FinalPage_RestartClicked;

		greetingTextsRandomList = new RandomList<string>(greetingTexts, false);

		StartCoroutine(enterRulePageCoroutine());
	}
	private IEnumerator enterRulePageCoroutine()
	{
		darkenPage.DarkenInstantly(1.0f);

		rulesPage.EnterPage(ruleText);

		yield return new WaitForSeconds(1.0f);

		yield return StartCoroutine(darkenPage.UndarkenScreen(0.7f));

		yield return new WaitForSeconds(2.0f);
	}

	private void RulesPage_StartGame(object sender, System.EventArgs e)
	{
		StartCoroutine(enterGamePageCoroutine());
	}

	private void FinalPage_RestartClicked(object sender, System.EventArgs e)
	{
		StartCoroutine(enterStartPageCoroutine());
	}

	private IEnumerator enterGamePageCoroutine()
	{
		rulesPage.ExitPage();
		gamePage.EnterPage();
		yield return new WaitForSeconds(0.3f);
		yield return StartCoroutine(rsgTimer.rsgStarting());

		stopMusic();

		preventGaming.SetActive(false);

		_gameManager.PlayGame();
	}

	private IEnumerator enterStartPageCoroutine()
	{
		stopMusic();
		yield return StartCoroutine(darkenPage.DarkenScreen(1.0f, 0.7f));

		gamePage.EnterPage();
		finalPage.ExitPage();
		rulesPage.EnterPage(ruleText);

		preventGaming.SetActive(true);

		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine(darkenPage.UndarkenScreen(0.7f));

		yield return new WaitForSeconds(2.0f);
	}

	private IEnumerator enterFinalPageCoroutine()
	{
		yield return new WaitForSeconds(1.5f);

		gamePage.ExitPage();

		finalPage.GreatingText(greetingTextsRandomList.Next());
		finalPage.EnterPage();
	}

	private void stopMusic()
	{
		player.Stop();
	}
}
