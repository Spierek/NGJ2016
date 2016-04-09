using UnityEngine;
using System.Collections.Generic;

public class ProgressBarScript : MonoBehaviour {
	#region Variables
	[SerializeField]
	private RectTransform m_ProgressBarBG;

	[SerializeField]
	private GameObject m_ProgressSectionPrefab;
	[SerializeField]
	private Transform m_ProgressSectionDir;

	private List<RectTransform> m_ProgressBarSections = new List<RectTransform>();
	#endregion

	#region Monobehaviour
	private void Awake()
	{
		CleanProgress();
	}
	#endregion

	#region Methods
	public void SetProgress(int val)
	{
		float progress = 1f - ((float)val / m_ProgressBarSections.Count);
		m_ProgressBarBG.localScale = new Vector3(progress, m_ProgressBarBG.localScale.y);
	}

	public void SetProgressLimit(int limit)
	{
		while (m_ProgressBarSections.Count < limit)
		{
			RectTransform t = LSUtils.InstantiateAndParent(m_ProgressSectionPrefab, m_ProgressSectionDir) as RectTransform;
			m_ProgressBarSections.Add(t);
		}
	}

	private void CleanProgress()
	{
		if (m_ProgressSectionDir.childCount > 0)
		{
			for (int i = 0; i < m_ProgressSectionDir.childCount; ++i)
			{
				Destroy(m_ProgressSectionDir.GetChild(i).gameObject, 0.01f);
			}
		}
	}
	#endregion
}