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
	[SerializeField] private GameMusicManager gameMusicManager;
	[SerializeField] private ReadySetGoTimer rsgTimer;
	[SerializeField] private GameScoresManager gameScoresManager;

	[SerializeField] private string[] greetingTexts;

	[SerializeField] private GameObject preventGaming;

	[SerializeField] private string ruleText;

	private IGameManager _gameManager;

	public float gameDelay = 5;

	private AudioSource player;

	private int _level;

	//private SaveLoadSettings settings = null;

	private RandomList<string> greetingTextsRandomList;

	// Start is called before the first frame update
	void Start()
	{
		_gameManager = gameManager;

		_gameManager.GameOver += _gameManager_GameOver;

		StartCoroutine(firstInitAndStartCoroutine());
	}

	private void _gameManager_GameOver(object sender, System.EventArgs e)
	{
		StartCoroutine(toFinalPageCoroutine());
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

		//player.volume = gameMusicManager.MusicVolume;
		playMusic(gameMusicManager.gameMenuList.Next());
	}

	private void RulesPage_StartGame(object sender, System.EventArgs e)
	{
		StartCoroutine(toGamePageCoroutine());
	}

	private void FinalPage_RestartClicked(object sender, System.EventArgs e)
	{
		StartCoroutine(toStartPageCoroutine());
	}

	private IEnumerator toGamePageCoroutine()
	{
		rulesPage.ExitPage();
		gamePage.EnterPage();
		yield return new WaitForSeconds(0.3f);
		yield return StartCoroutine(rsgTimer.rsgStarting());

		stopMusic();

		preventGaming.SetActive(false);

		playMusic(gameMusicManager.gameMusicList.Next());

		_gameManager.PlayGame();
	}

	private IEnumerator toStartPageCoroutine()
	{
		stopMusic();
		yield return StartCoroutine(darkenPage.DarkenScreen(1.0f, 0.7f));

		gameScoresManager.ResetScores();

		gamePage.EnterPage();
		finalPage.ExitPage();
		rulesPage.EnterPage(ruleText);

		preventGaming.SetActive(true);

		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine(darkenPage.UndarkenScreen(0.7f));

		yield return new WaitForSeconds(2.0f);

		playMusic(gameMusicManager.gameMenuList.Next());
	}

	private IEnumerator toFinalPageCoroutine()
	{
		yield return new WaitForSeconds(1.5f);

		gamePage.ExitPage();

		finalPage.GreatingText(greetingTextsRandomList.Next());
		finalPage.EnterPage();
	}

	private void playMusic(AudioClip clip)
	{
		player.clip = clip;
		player.Play();
	}

	private void stopMusic()
	{
		player.Stop();
	}
}
