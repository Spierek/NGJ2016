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
		Transform t = Instantiate(m_SplatPrefab).transform;
		t.position = position;
	}

	private IEnumerator FadeBG(Color prevColor, Color newColor)
	{
		float timer = 0;
		while (timer < m_TransitionDuration)
		{
			m_ArenaBG.color = Color.Lerp(prevColor, newColor, timer / m_TransitionDuration);
			timer += Time.deltaTime;
			yield return null;
		}

		m_ArenaBG.color = newColor;
	}
	#endregion
}