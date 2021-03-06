﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	#region Variables
	public static GameManager Instance;

	public PlayerController player;
	public EnemyManager enemyManager;
	public BoundsScript bounds;
	public EnemySpawner enemySpawner;
	public PaintManager paintManager;
	public UIManager uiManager;

	[Header("Audio")]
	public AudioSource whiteMusicSource;
	public AudioSource blackMusicSource;

	private int m_ProgressLimit = 10;
	private int m_CurrentProgress;

	private int m_TotalKills = 0;

	private bool m_TransitionNextFrame = false;
	private bool m_IsGameOver = false;
	#endregion

	#region Monobehaviour
	private void Awake() {
		Instance = this;
	}

	private void Start()
	{
		m_CurrentProgress = m_ProgressLimit;
		SetGameplayFreeze(true);
		uiManager.progressBar.SetProgressLimit(m_ProgressLimit);
		uiManager.progressBar.SetProgress(m_CurrentProgress);
	}

	private void Update()
	{
		if (Input.anyKeyDown)
		{
			StartCoroutine(StartGame());
		}

		if (m_IsGameOver)
		{
			if (Input.anyKeyDown)
			{
				SceneManager.LoadScene(0);
			}
		}
	}
	#endregion

	#region Methods
	public void SetGameplayFreeze(bool set)
	{
		player.SetFreeze(set);
		enemyManager.FreezeAllEnemies(set);
		enemySpawner.SetFreeze(set);
	}

	public void Progress()
	{
		m_CurrentProgress--;
		uiManager.progressBar.SetProgress(m_CurrentProgress);

		if (m_CurrentProgress <= 0 && !m_TransitionNextFrame)
		{
			StartCoroutine(NextStage());
			m_TransitionNextFrame = true;
		}

		m_TotalKills++;
		uiManager.totalCounter.text = m_TotalKills.ToString();
	}

	public IEnumerator GameOver()
	{
		SetGameplayFreeze(true);
		uiManager.gameOverTotalText.text = "SCORE: " + m_TotalKills;

		if (!ColorManager.Instance.GetIsBlack())
		{
			paintManager.StartTransition();
		}	
		uiManager.FadeHUDGroup(0f);
		uiManager.FadeGameOverGroup(1f);

		yield return new WaitForSeconds(1f);
		m_IsGameOver = true;
	}

	private IEnumerator NextStage()
	{
		yield return null;		// wait 1 frame
		paintManager.StartTransition();

		m_ProgressLimit += 2;
		m_CurrentProgress = m_ProgressLimit;

		uiManager.progressBar.SetProgressLimit(m_ProgressLimit);
		uiManager.progressBar.SetProgress(m_CurrentProgress);

		m_TransitionNextFrame = false;

		if (ColorManager.Instance.GetIsBlack())
		{
			whiteMusicSource.Stop();
			blackMusicSource.Play();
		}
		else
		{
			blackMusicSource.Stop();
			whiteMusicSource.Play();
		}
	}

	private IEnumerator StartGame()
	{
		uiManager.FadeHUDGroup(1f);
		uiManager.FadeTitleGroup(0f);

		yield return new WaitForSeconds(1f);
		SetGameplayFreeze(false);
	}
	#endregion
}