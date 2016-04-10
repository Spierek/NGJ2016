using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {
	#region Variables
	public static GameManager Instance;

	public PlayerController player;
	public EnemyManager enemyManager;
	public BoundsScript bounds;
	public EnemySpawner enemySpawner;
	public PaintManager paintManager;
	public UIManager uiManager;

	private int m_ProgressLimit = 10;
	private int m_CurrentProgress;

	private int m_TotalKills = 0;

	private bool m_TransitionNextFrame = false;
	#endregion

	#region Monobehaviour
	private void Awake() {
		Instance = this;
	}

	private void Start()
	{
		m_CurrentProgress = m_ProgressLimit;
		uiManager.progressBar.SetProgressLimit(m_ProgressLimit);
		uiManager.progressBar.SetProgress(m_CurrentProgress);
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

	private IEnumerator NextStage()
	{
		yield return null;		// wait 1 frame
		paintManager.StartTransition();

		m_ProgressLimit += 2;
		m_CurrentProgress = m_ProgressLimit;

		uiManager.progressBar.SetProgressLimit(m_ProgressLimit);
		uiManager.progressBar.SetProgress(m_CurrentProgress);

		m_TransitionNextFrame = false;
	}
	#endregion
}