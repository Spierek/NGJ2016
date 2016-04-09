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
		player.SetFreeze(set);
		enemyManager.FreezeAllEnemies(set);
		enemySpawner.SetFreeze(set);
	}
	#endregion
}