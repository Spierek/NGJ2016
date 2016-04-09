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
	#endregion

	#region Monobehaviour
	private void Awake() {
		Instance = this;
	}
	#endregion

	#region Methods
	public void SetGameplayFreeze(bool set)
	{
		if (Input.GetKeyDown(KeyCode.U))
		{
			player.SetFreeze(true);
			enemyManager.FreezeAllEnemies();
			enemySpawner.SetFreeze(true);
		}
		if (Input.GetKeyDown(KeyCode.I))
		{
			player.SetFreeze(false);
			enemyManager.UnfreezeAllEnemies();
			enemySpawner.SetFreeze(false);
		}
	}
	#endregion
}