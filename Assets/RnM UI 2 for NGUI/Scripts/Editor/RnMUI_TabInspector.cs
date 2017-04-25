using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(RnMUI_Tab))]
public class RnMUI_TabInspector : Editor
{
	RnMUI_Tab mTab;
	
	public override void OnInspectorGUI()
	{
		EditorGUIUtility.labelWidth = 100f;
		mTab = target as RnMUI_Tab;

		serializedObject.Update();

		EditorGUILayout.Space();
		NGUIEditorTools.DrawProperty("Target Content", serializedObject, "targetContent");
		EditorGUILayout.Space();
		NGUIEditorTools.DrawProperty("Link With Tab", serializedObject, "linkWith");
		EditorGUILayout.Space();

		if (NGUIEditorTools.DrawHeader("Tab Label"))
		{
			NGUIEditorTools.BeginContents();
			EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
			{
				NGUIEditorTools.DrawProperty("Label", serializedObject, "tabLabel");

				if (mTab.tabLabel != null)
				{
					EditorGUILayout.Space();

					SerializedObject obj = new SerializedObject(mTab.tabLabel);
					obj.Update();
					NGUIEditorTools.DrawProperty("Color Inactive", obj, "mColor");
					obj.ApplyModifiedProperties();

					NGUIEditorTools.DrawProperty("Color Active", serializedObject, "activeLabelColor");
					NGUIEditorTools.DrawProperty("Color Hovered", serializedObject, "hoverLabelColor");
					EditorGUILayout.Space();
					NGUIEditorTools.DrawProperty("Tween Duration", serializedObject, "labelTweenDuration");
				}
			}
			EditorGUI.EndDisabledGroup();
			NGUIEditorTools.EndContents();
		}

		if (NGUIEditorTools.DrawHeader("Tab Sprite"))
		{
			NGUIEditorTools.BeginContents();
			EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
			{
				NGUIEditorTools.DrawProperty("Tab Sprite", serializedObject, "tabSprite");

				if (mTab.tabSprite != null)
				{
					EditorGUILayout.Space();
					
					SerializedObject obj = new SerializedObject(mTab.tabSprite);
					obj.Update();
					NGUIEditorTools.DrawProperty("Color Normal", obj, "mColor");
					obj.ApplyModifiedProperties();

					NGUIEditorTools.DrawProperty("Color Hovered", serializedObject, "hoverSpriteColor");
					NGUIEditorTools.DrawProperty("Color Active", serializedObject, "activeSpriteColor");
					EditorGUILayout.Space();
					NGUIEditorTools.DrawProperty("Tween Duration", serializedObject, "spriteTweenDuration");
				}
			}
			EditorGUI.EndDisabledGroup();
			NGUIEditorTools.EndContents();
		}

		serializedObject.ApplyModifiedProperties();
	}
}