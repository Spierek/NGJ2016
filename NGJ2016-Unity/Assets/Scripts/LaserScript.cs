using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;

public class LaserScript : MonoBehaviour
{
	#region Variables
	[SerializeField]
	private Collider2D hitbox;
	[SerializeField]
	private Collider2D lavaTrigger;
	[SerializeField]
	private SpriteRenderer spriteRenderer;
	#endregion

	#region Monobehaviour
	private void Start()
	{
		SetColor();
		PaintManager.Instance.AddLaser(this);
		StartCoroutine(DelayedHitboxDisable());
	}
	#endregion

	#region Methods
	// TODO #LS pool paint instead of destroy?		
	public void DelayedDestroy(bool set, float delay)
	{
		lavaTrigger.enabled = set;
		Destroy(gameObject, delay);

		// delayed paint fadeout
		Sequence seq = DOTween.Sequence();
		seq.PrependInterval(0.7f * delay).Append(spriteRenderer.DOFade(0, 0.3f * delay));
		seq.Play();
	}

	private void SetColor()
	{
		SetColor(ColorManager.GetCurrentColor());
	}

	private void SetColor(Color col)
	{
		spriteRenderer.color = col;
	}

	private IEnumerator DelayedHitboxDisable()
	{
		yield return null;
		hitbox.enabled = false;
	}
	#endregion
}