using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaintManager : MonoBehaviour {
	#region Variables
	[Header("Time")]
	[SerializeField, Range(0, 3f)]
	private float m_TransitionDuration = 1f;

	[Header("Prefabs")]
	[SerializeField]
	private GameObject m_SplatPrefab;
	[SerializeField]
	private GameObject m_BigSplatPrefab;

	[Header("References")]
	[SerializeField]
	private SpriteRenderer m_ArenaBG;
	[SerializeField]
	private Transform m_PaintDir;

	private List<PaintInstance> m_Instances = new List<PaintInstance>();
	#endregion

	#region Methods
	public void StartTransition()
	{
		for (int i = 0; i < m_Instances.Count; ++i)
		{
			m_Instances[i].DelayedDestroy(false, m_TransitionDuration);
		}
		m_Instances.Clear();
		
		Color prevColor = m_ArenaBG.color;
		Color newColor = ColorManager.Instance.GetCurrentColor();
		ColorManager.Instance.ChangeCurrentColor();

		StartCoroutine(FadeBG(prevColor, newColor));
	}

	public void AddInstance(PaintInstance paint)
	{
		paint.transform.parent = m_PaintDir;
		m_Instances.Add(paint);
	}

	public void AddSplat(Vector3 position, Color color)
	{
		Transform t = LSUtils.InstantiateAndParent(m_SplatPrefab, m_PaintDir);
		t.position = position;
	}

	public void AddBigSplat(Color color, float lifetime)
	{
		GameObject go = Instantiate(m_BigSplatPrefab);
		go.transform.parent = m_PaintDir;
		go.GetComponent<TransitionPaint>().Enable(color, lifetime);
		
		Destroy(go, lifetime);
	}

	private IEnumerator FadeBG(Color prevColor, Color newColor)
	{
		GameManager.Instance.SetGameplayFreeze(true);
		CameraShaker.Instance.Shake(m_TransitionDuration, 2f);
		float timer = 0;

		while (timer < m_TransitionDuration)
		{
			float remaining = m_TransitionDuration - timer;
			AddBigSplat(newColor, remaining);
            AddBigSplat(newColor, remaining);


			timer += Time.deltaTime;
			yield return null;
		}

		m_ArenaBG.color = newColor;

		GameManager.Instance.SetGameplayFreeze(false);
	}
	#endregion
}