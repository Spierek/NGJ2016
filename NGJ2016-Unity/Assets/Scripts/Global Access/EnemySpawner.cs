using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct EnemySpawnData
{
	public GameObject prefab;
	[Range(0,1f)]
	public float spawnChance;
}

public class EnemySpawner : MonoBehaviour {
	#region Variables
	[SerializeField]
	private Vector2 m_SpawnDelay = new Vector2(1f, 2f);
	[SerializeField, Range(0,10f)]
	private float m_MinPlayerDistance = 1f;

	[SerializeField]
	private Transform m_SpawnDir;

	[SerializeField]
	private List<EnemySpawnData> m_EnemyPrefabs = new List<EnemySpawnData>();

	private bool m_IsFrozen = false;
	#endregion

	#region Monobehaviour
	private void Start() {
		StartCoroutine(WaitAndSpawn());
	}
	#endregion

	#region Methods
	public void SetFreeze(bool set)
	{
		m_IsFrozen = set;
	}

	private IEnumerator WaitAndSpawn()
	{
		float delay = Random.Range(m_SpawnDelay.x, m_SpawnDelay.y);
		float timer = 0f;

		while (timer < delay)
		{
			if (!m_IsFrozen)
			{
				timer += Time.deltaTime;
			}

			yield return null;
		}
		
		// check if spawning isn't happening too close to the player
		do
		{
			RandomizePosition();
		}
		while (CheckPlayerDistance());
		Spawn();

		// start another wait
		StartCoroutine(WaitAndSpawn());
	}

	private void RandomizePosition()
	{
		transform.position = GameManager.Instance.bounds.GetRandomPoint();
	}

	private bool CheckPlayerDistance()
	{
		return m_MinPlayerDistance >= Vector3.Distance(GameManager.Instance.player.transform.position, transform.position);
	}

	private void Spawn()
	{
		// get spawn chance
		float spawnRange = 0f;
		for (int i = 0; i < m_EnemyPrefabs.Count; ++i)
		{
			spawnRange += m_EnemyPrefabs[i].spawnChance;
		}

		if (spawnRange <= 0)
		{
			return;
		}

		float targetChance = Random.Range(0, spawnRange);

		// find target prefab
		float currentChance = 0f;
		for (int i = 0; i < m_EnemyPrefabs.Count; ++i)
		{
			currentChance += m_EnemyPrefabs[i].spawnChance;
			if (currentChance >= targetChance)
			{
				Transform t = LSUtils.InstantiateAndParent(m_EnemyPrefabs[i].prefab, m_SpawnDir);
				t.position = transform.position;
				GameManager.Instance.enemyManager.AddEnemy(t.GetComponent<BaseEnemy>());
				return;
			}
		}
	}
	#endregion
}