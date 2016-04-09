using UnityEngine;
using DG.Tweening;

public class PaintInstance : MonoBehaviour {
	#region Variables
	[SerializeField]
	private bool randomizeRotation = false;

	[Header("References")]
	[SerializeField]
	private Collider2D lavaTrigger;
	[SerializeField]
	private SpriteRenderer spriteRenderer;
	#endregion

	#region Monobehaviour
	private void Start() {
		SetColor();
		PaintManager.Instance.AddInstance(this);

		if (randomizeRotation)
		{
			Rotate();
		}
	}
	#endregion

	#region Methods
	// TODO #LS pool paint instead of destroy?		
	public void DelayedDestroy(bool set, float delay)
	{
		lavaTrigger.enabled = false;
		Destroy(gameObject, delay);

		// delayed paint fadeout
		Sequence seq = DOTween.Sequence();
		seq.PrependInterval(0.7f * delay).Append(spriteRenderer.DOFade(0, 0.3f * delay));
		seq.Play();
	}

	private void SetColor()
	{
		SetColor(ColorManager.Instance.GetCurrentColor());
	}

	private void SetColor(Color col)
	{
		spriteRenderer.color = col;
	}

	private void Rotate()
	{
		transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
	}
	#endregion
}