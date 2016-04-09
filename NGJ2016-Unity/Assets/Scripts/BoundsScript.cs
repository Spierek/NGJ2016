using UnityEngine;
using System.Collections.Generic;

public class BoundsScript : MonoBehaviour {
	#region Variables
	public static BoundsScript Instance;

	public Bounds bounds;
	#endregion

	#region Monobehaviour
	private void Awake()
	{
		Instance = this;
	}
	#endregion
}