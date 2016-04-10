using UnityEngine;
using System.Collections.Generic;

public class BoundsScript : MonoBehaviour {
	#region Variables
	public Bounds bounds;
	#endregion

	public Vector3 GetRandomPoint()
	{
		Bounds bounds = GameManager.Instance.bounds.bounds;
		Vector3 newPos = new Vector3(
			Random.Range(-bounds.extents.x, bounds.extents.x),
            Random.Range(-bounds.extents.y, bounds.extents.y),
			0);

		return newPos;
	}
}