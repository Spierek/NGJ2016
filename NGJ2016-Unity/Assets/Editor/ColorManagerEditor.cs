using UnityEditor;

[CustomEditor(typeof(ColorManager))]
public class ColorManagerEditor : Editor
{
	#region Methods
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (ColorManager.Instance != null)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Current Color");
			EditorGUILayout.ColorField(ColorManager.Instance.GetCurrentColor());
			EditorGUILayout.EndHorizontal();
		}
	}
	#endregion
}