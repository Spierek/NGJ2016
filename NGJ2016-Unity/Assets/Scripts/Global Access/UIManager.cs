using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour {
	#region Variables
	public Slider healthBar;
	public ProgressBarScript progressBar;
	public Text totalCounter;

	[Space(10)]
	[SerializeField]
	private CanvasGroup hudGroup;
	[SerializeField]
	private CanvasGroup titleGroup;
	[SerializeField]
	private CanvasGroup gameOverGroup;
	#endregion

	private void Start()
	{
		hudGroup.alpha = 0;
		titleGroup.alpha = 1;
		gameOverGroup.alpha = 0;
	}

	public void FadeHUDGroup(float target)
	{
		hudGroup.DOFade(target, 1f);
	}

	public void FadeTitleGroup(float target)
	{
		titleGroup.DOFade(target, 1f);
	}

	public void FadeGameOverGroup(float target)
	{
		gameOverGroup.DOFade(target, 1f);
	}
}