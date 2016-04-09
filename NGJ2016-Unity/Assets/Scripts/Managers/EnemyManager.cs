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

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.U))
		{
			FreezeAllEnemies();
			PlayerController.Instance.SetFreeze(true);
			EnemySpawner.Instance.SetFreeze(true);
		}
		if (Input.GetKeyDown(KeyCode.I))
		{
			UnfreezeAllEnemies();
			PlayerController.Instance.SetFreeze(false);
			EnemySpawner.Instance.SetFreeze(false);
		}
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
		for (int i = 0; i < m_Enemies.Count; ++i)
		{
			m_Enemies[i].SetFreeze(true);
		}
	}

	public void UnfreezeAllEnemies()
	{
		for (int i = 0; i < m_Enemies.Count; ++i)
		{
			m_Enemies[i].SetFreeze(false);
		}
	}
	#endregion
}