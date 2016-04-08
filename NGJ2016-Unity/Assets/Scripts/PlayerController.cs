using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : LSCacheBehaviour {
	#region Variables
	[SerializeField, Range(0,200)]
	private float acceleration = 10f;
	[SerializeField, Range(0,200)]
	private float friction = 70f;

	private const string HORIZONTAL_AXIS_NAME = "Horizontal";
	private const string VERTICAL_AXIS_NAME =	"Vertical";
	#endregion

	#region Monobehaviour
	private void Awake() {

	}
	
	private void Update() {
		rigidbody2D.drag = friction;
		Move();
	}
	#endregion

	#region Methods
	private void Move()
	{
		// get input
		Vector2 movement = Vector2.zero;
		movement.x = Input.GetAxisRaw(HORIZONTAL_AXIS_NAME);
		movement.y = Input.GetAxisRaw(VERTICAL_AXIS_NAME);

		// apply input
		rigidbody2D.AddForce(movement * acceleration * Time.deltaTime, ForceMode2D.Impulse);
	}
	#endregion
}