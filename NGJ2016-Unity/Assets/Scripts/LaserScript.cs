using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PaintInstance))]
public class LaserScript : MonoBehaviour
{
	#region Variables
	[SerializeField]
	private Collider2D hitbox;
	#endregion

	#region Monobehaviour
	private void Start()
	{
		StartCoroutine(DelayedHitboxDisable());
	}
	#endregion

	#region Methods
	private IEnumerator DelayedHitboxDisable()
	{
		yield return null;
		yield return null;
		hitbox.enabled = false;
	}
	#endregion
}