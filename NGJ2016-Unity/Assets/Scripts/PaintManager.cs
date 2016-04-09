using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaintManager : MonoBehaviour {
	#region Variables
	public static PaintManager Instance;

	[SerializeField, Range(0, 3f)]
	private float m_TransitionDuration = 1f;
	[SerializeField]
	private SpriteRenderer m_ArenaBG;
	[SerializeField]
	private Transform m_PaintDir;

	private List<LaserScript> m_Lasers = new List<LaserScript>();
	#endregion

	#region Monobehaviour
	private void Awake() {
		Instance = this;
	}
	#endregion

	#region Methods
	public void StartTransition()
	{
		for (int i = 0; i < m_Lasers.Count; ++i)
		{
			m_Lasers[i].DelayedDestroy(false, m_TransitionDuration);
		}
		m_Lasers.Clear();
		
		Color prevColor = m_ArenaBG.color;
		Color newColor = ColorManager.GetCurrentColor();
		ColorManager.ChangeCurrentColor();
		StartCoroutine(FadeBG(prevColor, newColor));
	}

	public void AddLaser(LaserScript laser)
	{
		m_Lasers.Add(laser);
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