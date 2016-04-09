using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {
	#region Variables
	public static EnemyManager Instance;

	private List<BaseEnemy> m_Enemies = new List<BaseEnemy>();
	#endregion

	#region Monobehaviour
	private void Awake() {
		Instance = this;
	}
	#endregion

	#region Methods
	public void AddEnemy(BaseEnemy enemy)
	{
		m_Enemies.Add(enemy);
	}

	public void RemoveEnemy(BaseEnemy enemy)
	{
		m_Enemies.Remove(enemy);
	}

	public void FreezeAllEnemies()
	{
		// TODO #LS
	}

	public void UnfreezeAllEnemies()
	{
		// TODO #LS
	}
	#endregion
}