using UnityEngine;
using System.Collections;

public class PainterEnemy : BaseEnemy {
	#region Variables
	[Space(10)]
	[SerializeField]
	private Sprite m_BlackSprite;
	[SerializeField]
	private Sprite m_WhiteSprite;

	[Space(10)]
	[SerializeField]
	private Vector2 m_DirectionChangeDuration = new Vector2(0.2f, 0.5f);
	[SerializeField]
	private Vector2 m_DirectionChangeDelay = new Vector2(1f, 3f);
	[SerializeField]
	private Vector2 m_PaintDelay = new Vector2(0.5f, 1f);

	[Space(10)]
	[SerializeField]
	private ParticleSystem m_Particles;

	private Vector3 m_Direction;
	#endregion

	#region Monobehaviour
	protected override void Start()
	{
		base.Start();
		StartCoroutine(RandomizeDirection());
		StartCoroutine(PlacePaint());
	}
	#endregion

	#region Methods
	public override void Kill(bool getPoint = true)
	{
		GameManager.Instance.paintManager.AddBigSplat(transform.position, m_SpriteRenderer.color);
		GameManager.Instance.enemyManager.RemoveEnemy(this, true);

		if (getPoint)
		{
			GameManager.Instance.Progress();
		}

		Destroy(gameObject);
	}

	protected override void Logic()
	{
		Move();
	}

	protected override void Move()
	{
		transform.position += new Vector3(m_Direction.x, m_Direction.y, 0) * m_MovementSpeed * Time.deltaTime;
	}

	protected override void SetColor()
	{
		m_SpriteRenderer.sprite = ColorManager.Instance.GetIsBlack() ? m_BlackSprite : m_WhiteSprite;
		m_Particles.startColor = ColorManager.Instance.GetCurrentColor();
	}

	private IEnumerator RandomizeDirection()
	{
		Vector3 oldDir = m_Direction;
		Vector3 target = GameManager.Instance.bounds.GetRandomPoint();
		Vector3 newDir = (target - transform.position).normalized;

		float timer = 0f;
		float duration = Random.Range(m_DirectionChangeDuration.x, m_DirectionChangeDuration.y);
		while (timer < duration)
		{
			m_Direction = Vector3.Lerp(oldDir, newDir, timer / duration);
			timer += Time.deltaTime;
			yield return null;
		}

		float delay = Random.Range(m_DirectionChangeDelay.x, m_DirectionChangeDelay.y);
		yield return new WaitForSeconds(delay);
		StartCoroutine(RandomizeDirection());
	}

	private IEnumerator PlacePaint()
	{
		float delay = Random.Range(m_PaintDelay.x, m_PaintDelay.y);
		yield return new WaitForSeconds(delay);

		GameManager.Instance.paintManager.AddSplat(transform.position, m_SpriteRenderer.color);
		StartCoroutine(PlacePaint());
	}
	#endregion
}