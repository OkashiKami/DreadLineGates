using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(RnMUI_RealmSelect_Realm))]
public class RnMUI_RealmSelect_RealmInspector : Editor
{
	RnMUI_RealmSelect_Realm mRealm;
	
	public override void OnInspectorGUI()
	{
		EditorGUIUtility.labelWidth = 100f;
		mRealm = target as RnMUI_RealmSelect_Realm;
		
		serializedObject.Update();
		
		EditorGUILayout.Space();
		NGUIEditorTools.DrawProperty("Target", serializedObject, "target");
		EditorGUILayout.Space();
		
		NGUIEditorTools.DrawProperty("Is Closed", serializedObject, "isClosed");
		NGUIEditorTools.DrawProperty("Status", serializedObject, "currentStatus");
		NGUIEditorTools.DrawProperty("Population", serializedObject, "currentPop");
		EditorGUILayout.Space();
		
		if (mRealm.target != null)
		{
			if (NGUIEditorTools.DrawHeader("Sprites"))
			{
				NGUIEditorTools.BeginContents();
				EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
				{
					SerializedObject obj = new SerializedObject(mRealm.target);
					obj.Update();
					
					GUILayout.BeginHorizontal();
					
					if (NGUIEditorTools.DrawPrefixButton("Atlas"))
						ComponentSelector.Show<UIAtlas>(OnSelectBackgroundAtlas);
					
					SerializedProperty atlas = NGUIEditorTools.DrawProperty("", obj, "mAtlas", GUILayout.MinWidth(20f));
					
					GUILayout.EndHorizontal();
					EditorGUILayout.Space();
					
					NGUIEditorTools.DrawSpriteField("Normal", obj, atlas, obj.FindProperty("mSpriteName"));
					
					obj.ApplyModifiedProperties();
					
					NGUIEditorTools.DrawSpriteField("Hovered", serializedObject, atlas, serializedObject.FindProperty("hoverSprite"), true);
					NGUIEditorTools.DrawSpriteField("Active", serializedObject, atlas, serializedObject.FindProperty("activeSprite"), true);
				}
				EditorGUI.EndDisabledGroup();
				NGUIEditorTools.EndContents();
			}
		}
		
		EditorGUILayout.Space();
		NGUIEditorTools.DrawProperty("Chars Label", serializedObject, "charsLabel");
		
		if (NGUIEditorTools.DrawHeader("Name Label"))
		{
			NGUIEditorTools.BeginContents();
			EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
			{
				NGUIEditorTools.DrawProperty("Label", serializedObject, "nameLabel");
				
				if (mRealm.nameLabel != null)
				{
					EditorGUILayout.Space();
					
					SerializedObject obj = new SerializedObject(mRealm.nameLabel);
					obj.Update();
					NGUIEditorTools.DrawProperty("Normal Color", obj, "mColor");
					obj.ApplyModifiedProperties();
					
					NGUIEditorTools.DrawProperty("Hover Color", serializedObject, "nameHoverColor");
					NGUIEditorTools.DrawProperty("Closed Color", serializedObject, "nameClosedColor");
				}
			}
			EditorGUI.EndDisabledGroup();
			NGUIEditorTools.EndContents();
		}
		
		if (NGUIEditorTools.DrawHeader("Status Label"))
		{
			NGUIEditorTools.BeginContents();
			EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
			{
				NGUIEditorTools.DrawProperty("Label", serializedObject, "statusLabel");
				
				if (mRealm.statusLabel != null)
				{
					EditorGUILayout.Space();
					
					NGUIEditorTools.DrawProperty("Online Color", serializedObject, "statusOnlineColor");
					NGUIEditorTools.DrawProperty("Offline Color", serializedObject, "statusOfflineColor");
				}
			}
			EditorGUI.EndDisabledGroup();
			NGUIEditorTools.EndContents();
		}
		
		if (NGUIEditorTools.DrawHeader("Population Label"))
		{
			NGUIEditorTools.BeginContents();
			EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
			{
				NGUIEditorTools.DrawProperty("Label", serializedObject, "populationLabel");
				
				if (mRealm.populationLabel != null)
				{
					EditorGUILayout.Space();
					
					NGUIEditorTools.DrawProperty("Low Color", serializedObject, "popLowColor");
					NGUIEditorTools.DrawProperty("Medium Color", serializedObject, "popMediumColor");
					NGUIEditorTools.DrawProperty("High Color", serializedObject, "popHighColor");
				}
			}
			EditorGUI.EndDisabledGroup();
			NGUIEditorTools.EndContents();
		}
		
		if (NGUIEditorTools.DrawHeader("Closed Label"))
		{
			NGUIEditorTools.BeginContents();
			EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
			{
				NGUIEditorTools.DrawProperty("Closed Label", serializedObject, "closedLabel");
				
				if (mRealm.closedLabel != null)
				{
					EditorGUILayout.Space();
					
					SerializedObject obj = new SerializedObject(mRealm.closedLabel);
					obj.Update();
					NGUIEditorTools.DrawProperty("Normal Color", obj, "mColor");
					obj.ApplyModifiedProperties();
					
					NGUIEditorTools.DrawProperty("Hover Color", serializedObject, "closedHoverColor");
				}
			}
			EditorGUI.EndDisabledGroup();
			NGUIEditorTools.EndContents();
		}
		
		serializedObject.ApplyModifiedProperties();
	}
	
	void RegisterUndo()
	{
		NGUIEditorTools.RegisterUndo("Realm Change", mRealm);
	}
	
	void OnSelectBackgroundAtlas(Object obj)
	{
		if (mRealm.target != null)
		{
			RegisterUndo();
			mRealm.target.atlas = obj as UIAtlas;
			NGUISettings.atlas = mRealm.target.atlas;
		}
	}
}