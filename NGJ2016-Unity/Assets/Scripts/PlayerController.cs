using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : LSCacheBehaviour
{
	#region Variables
	public static PlayerController Instance;

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

	[Header("Components")]
	[SerializeField]
	private Collider2D m_Hitbox;
	[SerializeField]
	private Transform m_CrosshairPivot;

	[Header("Prefabs")]
	[SerializeField]
	private GameObject m_LaserPrefab;

	private float m_CurrentHealth;

	private Vector3 m_WorldMousePos;
	private Vector2 m_Forward;
	private float m_RotationAngle;

	private bool m_HandleDOTInThisTurn = false;	// handle only one DamageOverTime event per turn
	#endregion

	#region Monobehaviour
	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		LSDebug.SetEnabled(true);
		SetHealth(m_MaxHealth);
	}

	private void Update()
	{
		UpdatePhysics();
		CalculateForward();
		PositionCrosshair();

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

	private void LateUpdate()
	{
		m_HandleDOTInThisTurn = true;
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer(GameConsts.LAVA_LAYER) && m_HandleDOTInThisTurn)
		{
			DamageOverTime();
		}
	}
	#endregion

	#region Methods
	public void Damage(float val)
	{
	 	SetHealth(m_CurrentHealth - val);
		if (m_CurrentHealth <= 0)
		{
			PaintManager.Instance.StartTransition();
			SetHealth(m_MaxHealth);		// TODO: nice lerped transition
		}
	}

	public float GetCurrentHealth01()
	{
		return m_CurrentHealth / m_MaxHealth;
	}

	private void DamageOverTime()
	{
		Damage(m_DamagePerSecond * Time.deltaTime);
		m_HandleDOTInThisTurn = false;
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

		Vector2 A = new Vector2(m_WorldMousePos.x, m_WorldMousePos.y);
		Vector2 B = new Vector2(transform.position.x, transform.position.y);
		m_Forward = (A - B).normalized;

		m_RotationAngle = -LSGamepad.GetStickAngle(m_Forward.x, m_Forward.y);
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
		// spawn player laser
		Transform t = Instantiate(m_LaserPrefab).transform;
		t.position = transform.position;
		t.localRotation = Quaternion.Euler(0, 0, m_RotationAngle);
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

	private void PositionCrosshair()
	{
		m_CrosshairPivot.localRotation = Quaternion.Euler(0, 0, m_RotationAngle);
	}

	private void UpdatePhysics()
	{
		rigidbody2D.drag = m_Friction;
	}
	#endregion
}