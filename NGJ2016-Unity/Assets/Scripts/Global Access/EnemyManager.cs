using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {
	#region Variables
	[SerializeField]
	protected AudioSource m_AudioSource;

	[SerializeField]
	private AudioClip m_BaseDeathSound;
	[SerializeField]
	private AudioClip m_PainterDeathSound;

	private List<BaseEnemy> m_Enemies = new List<BaseEnemy>();
	#endregion

	#region Methods
	public void AddEnemy(BaseEnemy enemy)
	{
		m_Enemies.Add(enemy);
	}

	public void RemoveEnemy(BaseEnemy enemy, bool isPainter = false)
	{
		m_Enemies.Remove(enemy);
		m_AudioSource.PlayOneShot(isPainter ? m_PainterDeathSound : m_BaseDeathSound);
	}

	public void FreezeAllEnemies(bool set)
	{
		for (int i = 0; i < m_Enemies.Count; ++i)
		{
			m_Enemies[i].SetFreeze(set);
		}
	}

	public void KillAllEnemies()
	{
		for (int i = 0; i < m_Enemies.Count; ++i)
		{
			m_Enemies[i].Kill(false);
		}
	}
	#endregion
}