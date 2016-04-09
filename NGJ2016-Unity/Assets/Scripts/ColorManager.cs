using UnityEngine;
using System.Collections.Generic;

public class ColorManager : MonoBehaviour {
	#region Variables
	private static Color currentColor;
	#endregion

	#region Monobehaviour
	private void Awake()
	{
		ChangeCurrentColor();
	}
	#endregion

	#region Methods
	public static Color GetCurrentColor()
	{
		return currentColor;
	}

	public static void ChangeCurrentColor()
	{
		currentColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f, 0.8f, 1f);
	}
	#endregion
}