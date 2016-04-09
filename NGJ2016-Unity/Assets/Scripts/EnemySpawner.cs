using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	#region Variables
	[SerializeField]
	private Vector2 spawnDelay = new Vector2(1f, 2f);
	[SerializeField, Range(0,10f)]
	private float minPlayerDistance = 1f;

	[SerializeField]
	private Transform spawnDir;

	[Header("Prefabs")]
	[SerializeField]
	private GameObject enemyPrefab;
	#endregion

	#region Monobehaviour
	private void Start() {
		StartCoroutine(WaitAndSpawn());
	}
	#endregion

	#region Methods
	private IEnumerator WaitAndSpawn()
	{
		float delay = Random.Range(spawnDelay.x, spawnDelay.y);
		yield return new WaitForSeconds(delay);

		// check if spawning isn't happening too close to the player
		do
		{
			RandomizePosition();
		}
		while (CheckPlayerDistance());

		Spawn();

		StartCoroutine(WaitAndSpawn());
	}

	private void RandomizePosition()
	{
		Bounds bounds = BoundsScript.Instance.bounds;
		Vector3 newPos = new Vector3(
			Random.Range(-bounds.extents.x, bounds.extents.x),
            Random.Range(-bounds.extents.y, bounds.extents.y),
			0);

		transform.position = newPos;
	}

	private bool CheckPlayerDistance()
	{
		return minPlayerDistance >= Vector3.Distance(PlayerController.Instance.transform.position, transform.position);
	}

	private void Spawn()
	{
		Transform t = LSUtils.InstantiateAndParent(enemyPrefab, spawnDir);
		t.position = transform.position;
		EnemyManager.Instance.AddEnemy(t.GetComponent<BaseEnemy>());
	}
	#endregion
}