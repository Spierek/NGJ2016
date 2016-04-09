using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PaintInstance))]
public class LaserScript : MonoBehaviour
{
	#region Variables
	[SerializeField]
	private Collider2D hitbox;
	[SerializeField]
	private Transform spritePivot;
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
		int duration = 4;
		for (int i = 0; i < duration; ++i)
		{
			spritePivot.localScale = new Vector3(1, (float)i / duration);
			if (i == 3)
			{
				hitbox.enabled = false;
			}

			yield return null;
		}

		spritePivot.localScale = new Vector3(1, 1f);
	}
	#endregion
}