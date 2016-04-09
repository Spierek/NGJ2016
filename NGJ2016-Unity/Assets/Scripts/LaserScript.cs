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
	}
	
	private void Update() {

	}
	#endregion

	#region Methods
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