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

	private int progressLimit = 10;
	private int currentProgress;

	private bool transitionNextFrame = false;
	#endregion

	#region Monobehaviour
	private void Awake() {
		Instance = this;
	}

	private void Start()
	{
		currentProgress = progressLimit;
		uiManager.progressBar.SetProgressLimit(progressLimit);
		uiManager.progressBar.SetProgress(currentProgress);
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
		currentProgress--;
		uiManager.progressBar.SetProgress(currentProgress);

		if (currentProgress <= 0 && !transitionNextFrame)
		{
			StartCoroutine(NextStage());
			transitionNextFrame = true;
		}
	}

	private IEnumerator NextStage()
	{
		yield return null;		// wait 1 frame
		paintManager.StartTransition();

		progressLimit += 2;
		currentProgress = progressLimit;

		uiManager.progressBar.SetProgressLimit(progressLimit);
		uiManager.progressBar.SetProgress(currentProgress);

		transitionNextFrame = false;
	}
	#endregion
}