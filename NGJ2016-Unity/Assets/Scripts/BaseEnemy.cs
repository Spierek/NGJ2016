using UnityEngine;
using System.Collections.Generic;

public class BaseEnemy : MonoBehaviour {
	#region Variables
	[Header("Movement")]
	public float movementSpeed = 10f;

	[SerializeField]
	private Collider2D hitbox;
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	private PlayerController player;
	#endregion

	#region Monobehaviour
	private void Start() {
		player = PlayerController.Instance;
		spriteRenderer.color = ColorManager.GetCurrentColor();
	}
	
	private void Update() {
		Move();
	}
	#endregion

	#region Methods
	private void Move()
	{
		Vector2 dir = (player.transform.position - transform.position).normalized;
		transform.position += new Vector3(dir.x, dir.y, 0) * movementSpeed * Time.deltaTime;
	}
	#endregion
}