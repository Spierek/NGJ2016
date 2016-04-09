using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	#region Variables
	[SerializeField]
	private Vector2 spawnRate = new Vector2(1f, 2f);
	[SerializeField]
	private Transform spawnDir;

	[Header("Prefabs")]
	[SerializeField]
	private GameObject enemyPrefab;
	#endregion

	#region Monobehaviour
	private void Start() {
		StartCoroutine(Spawn());
	}
	#endregion

	#region Methods
	private IEnumerator Spawn()
	{
		Transform t = LSUtils.InstantiateAndParent(enemyPrefab, spawnDir);
		t.position = transform.position;

		float delay = Random.Range(spawnRate.x, spawnRate.y);
		yield return new WaitForSeconds(delay);
		StartCoroutine(Spawn());
	}
	#endregion
}