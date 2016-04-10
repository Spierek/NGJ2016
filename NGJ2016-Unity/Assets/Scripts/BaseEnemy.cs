using UnityEngine;

public class BaseEnemy : MonoBehaviour {
	#region Variables
	[Header("Movement")]
	[SerializeField, Range(0, 10f)]
	protected float m_MovementSpeed = 10f;

	[SerializeField]
	protected SpriteRenderer m_SpriteRenderer;

	private bool m_IsFrozen = false;
	#endregion

	#region Monobehaviour
	protected virtual void Start() {
		SetColor();
	}
	
	private void Update() {
		if (!m_IsFrozen)
		{
			Logic();
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

	public virtual void Kill()
	{
		GameManager.Instance.paintManager.AddSplat(transform.position, m_SpriteRenderer.color);
		GameManager.Instance.enemyManager.RemoveEnemy(this);
		Destroy(gameObject);
	}

	protected virtual void Logic()
	{
		Move();
	}

	protected virtual void SetColor()
	{
		m_SpriteRenderer.color = ColorManager.Instance.GetCurrentColor();
	}

	protected virtual void Move()
	{
		Vector2 dir = (GameManager.Instance.player.transform.position - transform.position).normalized;
		transform.position += new Vector3(dir.x, dir.y, 0) * m_MovementSpeed * Time.deltaTime;
	}
	#endregion
}