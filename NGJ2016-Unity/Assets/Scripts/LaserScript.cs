using UnityEngine;
using System.Collections.Generic;

public class LaserScript : MonoBehaviour {
	#region Variables
	[SerializeField]
	private Collider2D hitbox;
	[SerializeField]
	private Collider2D lavaTrigger;
	[SerializeField]
	private SpriteRenderer spriteRenderer;
	#endregion

	#region Monobehaviour
	private void Start() {
		SetColor();
		PaintManager.Instance.AddLaser(this);
	}
	
	private void Update() {

	}
	#endregion

	#region Methods
	// TODO #LS pool paint instead of destroy?		
	public void DelayedDestroy(bool set, float delay)
	{
		lavaTrigger.enabled = set;
		Destroy(gameObject, delay);
	}

	private void SetColor()
	{
		SetColor(ColorManager.GetCurrentColor());
	}

	private void SetColor(Color col)
	{
		spriteRenderer.color = col;
	}
	#endregion
}