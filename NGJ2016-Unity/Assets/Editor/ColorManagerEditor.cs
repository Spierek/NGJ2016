﻿using UnityEditor;

[CustomEditor(typeof(ColorManager))]
public class ColorManagerEditor : Editor
{
	#region Methods
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Current Color");
		EditorGUILayout.ColorField(ColorManager.GetCurrentColor());
		EditorGUILayout.EndHorizontal();
	}
	#endregion
}