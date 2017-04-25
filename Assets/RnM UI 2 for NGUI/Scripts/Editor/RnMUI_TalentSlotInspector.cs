using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(RnMUI_TalentSlot))]
public class RnMUI_TalentSlotInspector : RnMUI_IconSlotInspector
{
	public override void OnInspectorGUI()
	{
		EditorGUIUtility.labelWidth = 120f;
		EditorGUILayout.Space();
		
		serializedObject.Update();
		NGUIEditorTools.DrawProperty("Points Label", serializedObject, "pointsLabel");
		serializedObject.ApplyModifiedProperties();
		
		base.DrawTargetSprite();
		this.DrawPointLabelOptions();
		base.DrawBehaviour();
	}
	
	public void DrawPointLabelOptions()
	{
		serializedObject.Update();
		
		if (NGUIEditorTools.DrawHeader("Points Label Colors"))
		{
			NGUIEditorTools.BeginContents();
			EditorGUIUtility.labelWidth = 150f;
			GUILayout.BeginVertical();
			
			NGUIEditorTools.DrawProperty("Min Color", serializedObject, "pointLabelMinColor");
			NGUIEditorTools.DrawProperty("Max Color", serializedObject, "pointLabelMaxColor");
			NGUIEditorTools.DrawProperty("Active Color", serializedObject, "pointLabelActiveColor");
			
			GUILayout.EndVertical();
			NGUIEditorTools.EndContents();
		}
		
		serializedObject.ApplyModifiedProperties();
	}
}