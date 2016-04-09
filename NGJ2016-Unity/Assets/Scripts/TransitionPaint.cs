using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class TransitionPaint : MonoBehaviour {
	#region Methods
	public void Enable(Color color, float lifetime)
	{
		Bounds b = GameManager.Instance.bounds.bounds;
		Vector3 tempV3 = new Vector3();
		tempV3.x = Random.Range(-b.extents.x, b.extents.x);
		tempV3.y = Random.Range(-b.extents.y, b.extents.y);
		tempV3.z = 0;

		transform.position = tempV3;

		tempV3 = transform.localScale * Random.Range(0.8f, 1.2f);
		transform.DOPunchScale(tempV3, 0.5f, 0, 0f);

		SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
		sr.color = color;
	}
	#endregion
}