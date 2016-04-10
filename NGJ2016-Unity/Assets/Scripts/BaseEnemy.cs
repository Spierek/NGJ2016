using UnityEngine;

public class BaseEnemy : MonoBehaviour {
	#region Variables
	[SerializeField, Range(0, 10f)]
	protected float m_MovementSpeed = 3f;
	[SerializeField, Range(10, 40f)]
	protected float m_Pushback = 30f;
	[SerializeField, Range(0, 50f)]
	protected float m_PlayerDamage = 5f;

	[SerializeField]
	protected SpriteRenderer m_SpriteRenderer;

	private bool m_IsFrozen = false;
	private Vector3 m_MoveDir = new Vector3();
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

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (!m_IsFrozen && collision.gameObject.layer == LayerMask.NameToLayer(GameConsts.PLAYER_LAYER))
		{
			GameManager.Instance.player.Damage(m_PlayerDamage);
			GameManager.Instance.player.GetComponent<Rigidbody2D>().AddForce(m_MoveDir * m_Pushback, ForceMode2D.Impulse);
			Kill(false);
		}
	}
	#endregion

	#region Methods
	public void SetFreeze(bool set)
	{
		m_IsFrozen = set;
	}

	public virtual void Kill(bool getPoint = true)
	{
		GameManager.Instance.paintManager.AddSplat(transform.position, m_SpriteRenderer.color);
		GameManager.Instance.enemyManager.RemoveEnemy(this);

		if (getPoint)
		{
			GameManager.Instance.Progress();
		}

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
		m_MoveDir = (GameManager.Instance.player.transform.position - transform.position).normalized;
		transform.position += new Vector3(m_MoveDir.x, m_MoveDir.y, 0) * m_MovementSpeed * Time.deltaTime;
	}
	#endregion
}