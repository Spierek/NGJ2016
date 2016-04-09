using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : LSCacheBehaviour
{
	#region Variables
	[Header("Movement")]
	[SerializeField, Range(1,200)]
	private float acceleration = 10f;
	[SerializeField, Range(1,200)]
	private float friction = 70f;
	[SerializeField, Range(1,100)]
	private float dashSpeed = 80f;

	[Header("Collision")]
	[SerializeField]
	private Collider2D hitbox;

	[Header("Prefabs")]
	[SerializeField]
	private GameObject laserPrefab;

	[Header("Dirs")]
	[SerializeField]
	private Transform paintDir;

	private Vector3 worldMousePos;
	private Vector3 forward;
	#endregion

	#region Monobehaviour
	private void Start()
	{
		LSDebug.SetEnabled(true);
	}

	private void Update()
	{
		rigidbody2D.drag = friction;
		CalculateForward();
		Move();

		if (Input.GetMouseButtonDown(0))
		{
			Shoot();
		}
		if (Input.GetMouseButtonDown(1))
		{
			Dash();
		}
	}

	public void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer(GameConsts.LAVA_LAYER_NAME))
		{
			LSDebug.WriteLine("LAVA");
		}
	}
	#endregion

	#region Methods
	private void CalculateForward()
	{
		Vector3 mousePos = Input.mousePosition;
		worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
		forward = worldMousePos - transform.position;
		forward.Normalize();
	}

	private void Move()
	{
		// get input
		Vector2 movement = Vector2.zero;
		movement.x = Input.GetAxisRaw(GameConsts.HORIZONTAL_AXIS_NAME);
		movement.y = Input.GetAxisRaw(GameConsts.VERTICAL_AXIS_NAME);

		// apply input
		rigidbody2D.AddForce(movement * acceleration * Time.deltaTime, ForceMode2D.Impulse);
	}

	private void Shoot()
	{
		// calculate rotation angle
		float rotationAngle = -LSGamepad.GetStickAngle(forward.x, forward.y);

		// spawn player laser
		Transform t = LSUtils.InstantiateAndParent(laserPrefab, paintDir);
		t.position = transform.position;
		t.localRotation = Quaternion.Euler(0, 0, rotationAngle);
    }

	private void Dash()
	{
		rigidbody2D.AddForce(forward * dashSpeed, ForceMode2D.Impulse);
		StartCoroutine(DisableHitboxDuringDash());
	}

	private IEnumerator DisableHitboxDuringDash()
	{
		hitbox.enabled = false;
		yield return new WaitForSeconds(2.0f / friction);
		hitbox.enabled = true;
	}
	#endregion
}