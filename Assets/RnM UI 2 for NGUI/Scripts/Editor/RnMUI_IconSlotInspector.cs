using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(RnMUI_IconSlot))]
public class RnMUI_IconSlotInspector : Editor
{
	public override void OnInspectorGUI()
	{
		EditorGUIUtility.labelWidth = 120f;
		EditorGUILayout.Space();

		DrawTargetSprite();
		DrawDragAndDrop();
		DrawBehaviour();
	}

	public void DrawTargetSprite()
	{
		serializedObject.Update();
		NGUIEditorTools.DrawProperty("Icon Sprite", serializedObject, "iconSprite");
		serializedObject.ApplyModifiedProperties();
	}

	public void DrawDragAndDrop()
	{
		RnMUI_IconSlot mSlot = target as RnMUI_IconSlot;
		serializedObject.Update();

		NGUIEditorTools.DrawProperty("Drag and Drop", serializedObject, "dragAndDropEnabled");
		
		// When drag and drop is enabled
		if (mSlot.dragAndDropEnabled)
		{
			NGUIEditorTools.DrawProperty("Allow throw away", serializedObject, "AllowThrowAway");
			NGUIEditorTools.DrawProperty("Is Static", serializedObject, "IsStatic");
			
			if (mSlot.IsStatic)
				EditorGUILayout.HelpBox("Static slots are intended to be used for spell books and such because they will not be unassigned when drag is strated.", MessageType.Warning);
		}
		
		serializedObject.ApplyModifiedProperties();
	}

	public void DrawBehaviour()
	{
		RnMUI_IconSlot mSlot = target as RnMUI_IconSlot;

		serializedObject.Update();
		
		if (NGUIEditorTools.DrawHeader("Behaviour"))
		{
			NGUIEditorTools.BeginContents();
			EditorGUIUtility.labelWidth = 150f;
			GUILayout.BeginVertical();
			
			// Hover Effect
			NGUIEditorTools.DrawProperty("Hover Effect Type", serializedObject, "hoverEffectType");

			if (mSlot.hoverEffectType == RnMUI_IconSlot.HoverEffectType.Sprite)
			{
				NGUIEditorTools.DrawProperty("Hover Sprite", serializedObject, "hoverEffectSprite");
			}
			else if (mSlot.hoverEffectType == RnMUI_IconSlot.HoverEffectType.Color)
			{
				NGUIEditorTools.DrawProperty("Hover Color", serializedObject, "hoverEffectColor");
			}
			
			// Hover effect speed value
			if (mSlot.hoverEffectType != RnMUI_IconSlot.HoverEffectType.None)
			{
				NGUIEditorTools.DrawProperty("Hover Tween Duration", serializedObject, "hoverEffectSpeed");
			}
			
			GUILayout.Space(10f);
			GUILayout.EndVertical();
			
			// Press Effect
			NGUIEditorTools.DrawProperty("Press Effect Type", serializedObject, "pressEffectType");
			
			if (mSlot.pressEffectType == RnMUI_IconSlot.PressEffectType.Sprite)
			{
				NGUIEditorTools.DrawProperty("Press Sprite", serializedObject, "pressEffectSprite");
			}
			else if (mSlot.pressEffectType == RnMUI_IconSlot.PressEffectType.Color)
			{
				NGUIEditorTools.DrawProperty("Press Color", serializedObject, "pressEffectColor");
			}
			
			// Press effect speed value
			if (mSlot.pressEffectType != RnMUI_IconSlot.PressEffectType.None)
			{
				NGUIEditorTools.DrawProperty("Press Tween Duration", serializedObject, "pressEffectSpeed");
				NGUIEditorTools.DrawProperty("Press Tween Insta Out", serializedObject, "pressEffectInstaOut");
			}
			
			NGUIEditorTools.EndContents();
		}
		
		serializedObject.ApplyModifiedProperties();
	}
}
