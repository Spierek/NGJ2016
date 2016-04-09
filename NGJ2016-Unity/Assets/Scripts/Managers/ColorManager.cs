using UnityEngine;

public class ColorManager : MonoBehaviour {
	#region Variables
	public static ColorManager Instance;

	[SerializeField]
	private Vector2 saturationRange = new Vector2(0.8f, 1f);
	[SerializeField]
	private Vector2 alphaRange = new Vector2(0.8f, 0.9f);

	private Color currentColor;
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
		return currentColor;
	}

	public void ChangeCurrentColor()
	{
		currentColor = Random.ColorHSV(0f, 1f, 
			saturationRange.x, saturationRange.y, 
			0.8f, 1f, 
			alphaRange.x, alphaRange.y);
	}
	#endregion
}