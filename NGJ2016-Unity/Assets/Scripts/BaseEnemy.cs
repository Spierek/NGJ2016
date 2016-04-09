using UnityEngine;
using System.Collections.Generic;

public class BaseEnemy : LSCacheBehaviour {
	#region Variables
	[Header("Movement")]
	[SerializeField, Range(0, 10f)]
	private float m_MovementSpeed = 10f;

	[SerializeField]
	private SpriteRenderer m_SpriteRenderer;

	private bool m_IsFrozen = false;
	#endregion

	#region Monobehaviour
	private void Start() {
		m_SpriteRenderer.color = ColorManager.Instance.GetCurrentColor();
	}
	
	private void Update() {
		if (!m_IsFrozen)
		{
			Move();
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!m_IsFrozen && collision.gameObject.layer == LayerMask.NameToLayer(GameConsts.PLAYER_PROJECTILE_LAYER))
		{
			Kill();
		}
	}
	#endregion

	#region Methods
	public void SetFreeze(bool set)
	{
		m_IsFrozen = set;
	}

	public void Kill()
	{
		// TODO #LS drop particles n shit
		PaintManager.Instance.AddSplat(transform.position, m_SpriteRenderer.color);
		EnemyManager.Instance.RemoveEnemy(this);
		Destroy(gameObject);
	}

	private void Move()
	{
		Vector2 dir = (PlayerController.Instance.transform.position - transform.position).normalized;
		transform.position += new Vector3(dir.x, dir.y, 0) * m_MovementSpeed * Time.deltaTime;
	}
	#endregion
}