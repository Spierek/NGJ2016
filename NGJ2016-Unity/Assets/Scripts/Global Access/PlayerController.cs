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
	[SerializeField, Range(0,2f)]
	private float m_DashDelay = 0.5f;
	[SerializeField, Range(0,2f)]
	private float m_FiringDelay = 0.2f;

	[Header("Components")]
	[SerializeField]
	private Collider2D m_Hitbox;
	[SerializeField]
	private Transform m_CrosshairPivot;
	[SerializeField]
	private Transform m_SpriteTransform;
	[SerializeField]
	private Animator m_Animator;

	[Header("Prefabs")]
	[SerializeField]
	private GameObject m_LaserPrefab;

	private float m_CurrentHealth;

	private Vector3 m_WorldMousePos;
	private Vector2 m_Forward;
	private float m_RotationAngle;

	private Vector3 m_InitialScale;

	private bool m_HandleDOTInThisTurn = true; // handle only one DamageOverTime event per turn
	private bool m_CanDash = true;
	private bool m_CanFire = true;
	private bool m_IsFrozen = false;
	#endregion

	#region Monobehaviour
	private void Awake()
	{
		m_InitialScale = m_SpriteTransform.localScale;
	}

	private void Start()
	{
		LSDebug.SetEnabled(true);
		SetHealth(m_MaxHealth);
	}

	private void Update()
	{
		if (!m_IsFrozen)
		{
			UpdatePhysics();
			CalculateForward();

			Move();

			if (Input.GetMouseButtonDown(0) && m_CanFire)
			{
				Shoot();
			}
			if (Input.GetMouseButtonDown(1) && m_CanDash)
			{
				Dash();
			}
		}

		RotatePlayerAndCrosshair();
	}

	private void LateUpdate()
	{
		m_HandleDOTInThisTurn = true;

		CheckBounds();
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
	public void SetFreeze(bool set)
	{
		m_IsFrozen = set;
		m_CrosshairPivot.gameObject.SetActive(!set);
	}

	public void Damage(float val)
	{
	 	SetHealth(m_CurrentHealth - val);
		if (m_CurrentHealth <= 0)
		{
			StartCoroutine(GameManager.Instance.GameOver());
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
		GameManager.Instance.uiManager.healthBar.value = GetCurrentHealth01();
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

		// animation
		LSDebug.WriteLine("mag", rigidbody2D.velocity.magnitude.ToString());
		m_Animator.SetBool("isRunning", rigidbody2D.velocity.magnitude > 0.1f);
	}
 
	private void Shoot()
	{
		// spawn player laser
		Transform t = Instantiate(m_LaserPrefab).transform;
		t.position = transform.position;
		t.localRotation = Quaternion.Euler(0, 0, m_RotationAngle);

		StartCoroutine(FiringDelay());
		CameraShaker.Instance.Shake(0.15f, 0.2f);
    }

	private void Dash()
	{
		rigidbody2D.AddForce(m_Forward * m_DashSpeed, ForceMode2D.Impulse);
		StartCoroutine(DisableHitboxDuringDash());
		StartCoroutine(DashDelay());
	}

	private void RotatePlayerAndCrosshair()
	{
		Vector3 newScale = m_InitialScale;
		newScale.x *= m_RotationAngle > 0 ? -1 : 1;
		m_SpriteTransform.localScale = newScale;

		m_CrosshairPivot.localRotation = Quaternion.Euler(0, 0, m_RotationAngle);
	}

	private void UpdatePhysics()
	{
		rigidbody2D.drag = m_Friction;
	}

	private IEnumerator DisableHitboxDuringDash()
	{
		m_Hitbox.enabled = false;
		yield return new WaitForSeconds(2.0f / m_Friction);
		m_Hitbox.enabled = true;
	}

	private IEnumerator DashDelay()
	{
		m_CanDash = false;
		yield return new WaitForSeconds(m_DashDelay);
		m_CanDash = true;
	}

	private IEnumerator FiringDelay()
	{
		m_CanFire = false;
		yield return new WaitForSeconds(m_FiringDelay);
		m_CanFire = true;
	}

	private void CheckBounds()
	{
		Bounds b = GameManager.Instance.bounds.bounds;
		Vector3 newPos = transform.position;
		newPos.x = Mathf.Clamp(newPos.x, -b.extents.x, b.extents.x);
		newPos.y = Mathf.Clamp(newPos.y, -b.extents.y, b.extents.y);
		LSDebug.WriteLine(b.extents.ToString());

		transform.position = newPos;
	}
	#endregion
}