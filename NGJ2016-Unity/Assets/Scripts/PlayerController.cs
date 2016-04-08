using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : LSCacheBehaviour {
	#region Variables
	[Header("Movement")]
	[SerializeField, Range(0,200)]
	private float acceleration = 10f;
	[SerializeField, Range(0,200)]
	private float friction = 70f;

	[Header("Prefabs")]
	[SerializeField]
	private GameObject laserPrefab;

	[Header("Dirs")]
	[SerializeField]
	private Transform paintDir;

	private const string HORIZONTAL_AXIS_NAME = "Horizontal";
	private const string VERTICAL_AXIS_NAME =	"Vertical";
	#endregion

	#region Monobehaviour
	private void Start() {
		LSDebug.SetEnabled(true);
	}
	
	private void Update() {
		rigidbody2D.drag = friction;
		Move();

		if (Input.GetMouseButtonDown(0))
		{
			Shoot();
		}
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

	private void Shoot()
	{
		// get firing direction
		Vector3 mousePos = Input.mousePosition;
		Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
		Vector3 forward = worldMousePos - transform.position;

		// calculate rotation angle
		float rotationAngle = -LSGamepad.GetStickAngle(forward.x, forward.y);

		Debug.DrawLine(transform.position, worldMousePos, Color.red);

		// spawn player laser
		Transform t = LSUtils.InstantiateAndParent(laserPrefab, paintDir);
		t.position = transform.position;
		t.localRotation = Quaternion.Euler(0, 0, rotationAngle);
	}
	#endregion
}