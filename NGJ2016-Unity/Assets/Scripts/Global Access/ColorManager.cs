using UnityEngine;

public class ColorManager : MonoBehaviour {
	#region Variables
	public static ColorManager Instance;

	[SerializeField]
	private Vector2 saturationRange = new Vector2(0.8f, 1f);
	[SerializeField]
	private Vector2 alphaRange = new Vector2(0.8f, 0.9f);

	private Color currentColor;
	private bool bgIsBlack = false;
	#endregion

	#region Monobehaviour
	private void Awake()
	{
		Instance = this;
		ChangeCurrentColor();
	}
	#endregion

	#region Methods
	public Color GetCurrentColor()
	{
		return bgIsBlack ? Color.black : Color.white;
	}

	public void ChangeCurrentColor()
	{
		bgIsBlack = !bgIsBlack;
	}

	public bool GetIsBlack()
	{
		return bgIsBlack;
	}
	#endregion
}