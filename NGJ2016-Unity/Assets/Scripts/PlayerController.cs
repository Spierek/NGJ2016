using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : LSCacheBehaviour
{
	#region Variables
	[Header("Stats")]
	[SerializeField, Range(0,100)]
	private float m_MaxHealth = 100;
	[SerializeField, Range(0,100)]
	private float m_DamagePerSecond = 10f;

	[Header("Movement")]
	[SerializeField, Range(1,200)]
	private float m_Acceleration = 150f;
	[SerializeField, Range(1,200)]
	private float m_Friction = 20f;
	[SerializeField, Range(1,200)]
	private float m_DashSpeed = 100f;

	[Header("Collision")]
	[SerializeField]
	private Collider2D m_Hitbox;

	[Header("Prefabs")]
	[SerializeField]
	private GameObject m_LaserPrefab;

	[Header("Dirs")]
	[SerializeField]
	private Transform m_PaintDir;

	private float m_CurrentHealth;

	private Vector3 m_WorldMousePos;
	private Vector3 m_Forward;
	#endregion

	#region Monobehaviour
	private void Start()
	{
		LSDebug.SetEnabled(true);
		SetHealth(m_MaxHealth);
	}

	private void Update()
	{
		rigidbody2D.drag = m_Friction;
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
			DamageOverTime();
		}
	}
	#endregion

	#region Methods
	public void Damage(float val)
	{
	 	SetHealth(m_CurrentHealth - val);
	}

	public float GetCurrentHealth01()
	{
		return m_CurrentHealth / m_MaxHealth;
	}

	private void DamageOverTime()
	{
		Damage(m_DamagePerSecond * Time.deltaTime);
	}

	private void SetHealth(float val)
	{
		m_CurrentHealth = Mathf.Clamp(val, 0, m_MaxHealth);
		UIManager.Instance.transitionSlider.value = GetCurrentHealth01();
	}

	private void CalculateForward()
	{
		Vector3 mousePos = Input.mousePosition;
		m_WorldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
		m_Forward = m_WorldMousePos - transform.position;
		m_Forward.Normalize();
	}

	private void Move()
	{
		// get input
		Vector2 movement = Vector2.zero;
		movement.x = Input.GetAxisRaw(GameConsts.HORIZONTAL_AXIS_NAME);
		movement.y = Input.GetAxisRaw(GameConsts.VERTICAL_AXIS_NAME);

		// apply input
		rigidbody2D.AddForce(movement * m_Acceleration * Time.deltaTime, ForceMode2D.Impulse);
	}

	private void Shoot()
	{
		// calculate rotation angle
		float rotationAngle = -LSGamepad.GetStickAngle(m_Forward.x, m_Forward.y);

		// spawn player laser
		Transform t = LSUtils.InstantiateAndParent(m_LaserPrefab, m_PaintDir);
		t.position = transform.position;
		t.localRotation = Quaternion.Euler(0, 0, rotationAngle);
    }

	private void Dash()
	{
		rigidbody2D.AddForce(m_Forward * m_DashSpeed, ForceMode2D.Impulse);
		StartCoroutine(DisableHitboxDuringDash());
	}

	private IEnumerator DisableHitboxDuringDash()
	{
		m_Hitbox.enabled = false;
		yield return new WaitForSeconds(2.0f / m_Friction);
		m_Hitbox.enabled = true;
	}
	#endregion
}