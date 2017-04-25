#if !UNITY_3_5 && !UNITY_FLASH
#define DYNAMIC_FONT
#endif

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Inspector class used to edit UISelectFields.
/// </summary>

[CustomEditor(typeof(UISelectField))]
public class UISelectFieldInspector : UIWidgetContainerEditor
{
	enum FontType
	{
		Bitmap,
		Dynamic,
	}
	
	UISelectField mList;
	FontType mType;
	
	void OnEnable ()
	{
		SerializedProperty bit = serializedObject.FindProperty("bitmapFont");
		mType = (bit.objectReferenceValue != null) ? FontType.Bitmap : FontType.Dynamic;
		mList = target as UISelectField;
		
		if (mList.ambigiousFont == null)
		{
			mList.ambigiousFont = NGUISettings.ambigiousFont;
			mList.fontSize = NGUISettings.fontSize;
			mList.fontStyle = NGUISettings.fontStyle;
			EditorUtility.SetDirty(mList);
		}
		
		if (mList.atlas == null)
		{
			mList.atlas = NGUISettings.atlas;
			mList.listBackgroundSprite = NGUISettings.selectedSprite;
			mList.listHighlightSprite = NGUISettings.selectedSprite;
			EditorUtility.SetDirty(mList);
		}
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		NGUIEditorTools.SetLabelWidth(80f);
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(6f);
		GUILayout.Label("Options");
		GUILayout.EndHorizontal();

		string text = "";
		foreach (string s in mList.items) text += s + "\n";
		
		GUILayout.Space(-14f);
		GUILayout.BeginHorizontal();
		GUILayout.Space(84f);
		string modified = EditorGUILayout.TextArea(text, GUI.skin.textArea, GUILayout.Height(100f));
		GUILayout.EndHorizontal();

		if (modified != text)
		{
			RegisterUndo();
			string[] split = modified.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
			mList.items.Clear();
			foreach (string s in split) mList.items.Add(s);
			
			if (string.IsNullOrEmpty(mList.value) || !mList.items.Contains(mList.value))
			{
				mList.value = mList.items.Count > 0 ? mList.items[0] : "";
			}
		}
		
		GUI.changed = false;
		string sel = NGUIEditorTools.DrawList("Default", mList.items.ToArray(), mList.value);
		if (GUI.changed) serializedObject.FindProperty("mSelectedItem").stringValue = sel;
		
		NGUIEditorTools.DrawProperty("Position", serializedObject, "position");
		NGUIEditorTools.DrawProperty("Localized", serializedObject, "isLocalized");
		
		EditorGUILayout.Space();
		
		DrawLabelLayout();
		DrawSelectField();
		DrawArrowSprites();
		DrawListLayout();
		DrawListHighlightLayout();
		DrawFont();
		
		NGUIEditorTools.DrawEvents("On Value Change", mList, mList.onChange);
		
		serializedObject.ApplyModifiedProperties();
	}

	void DrawLabelLayout()
	{
		if (NGUIEditorTools.DrawHeader("Label Layout"))
		{
			NGUIEditorTools.BeginContents();
			
			NGUIEditorTools.DrawProperty("Label", serializedObject, "labelElement");
			
			if (mList.labelElement != null)
			{
				EditorGUILayout.Space();
				
				SerializedObject obj = new SerializedObject(mList.labelElement);
				obj.Update();
				
				NGUIEditorTools.DrawProperty("Color Normal", obj, "mColor");
				NGUIEditorTools.DrawProperty("Color Focused", serializedObject, "labelFocusedColor");
				
				obj.ApplyModifiedProperties();
			}
			
			NGUIEditorTools.EndContents();
		}
	}

	void DrawSelectField()
	{
		NGUIEditorTools.SetLabelWidth(100f);
		
		if (NGUIEditorTools.DrawHeader("Select Field Layout"))
		{
			NGUIEditorTools.BeginContents();
			
			NGUIEditorTools.DrawProperty("Background Sprite", serializedObject, "backgroundElement");
			
			if (mList.backgroundElement != null)
			{
				EditorGUILayout.Space();
				
				SerializedObject obj = new SerializedObject(mList.backgroundElement);
				obj.Update();
				
				GUILayout.BeginHorizontal();
				
				if (NGUIEditorTools.DrawPrefixButton("Atlas"))
					ComponentSelector.Show<UIAtlas>(OnSelectBackgroundAtlas);
				
				SerializedProperty atlas = NGUIEditorTools.DrawProperty("", obj, "mAtlas", GUILayout.MinWidth(20f));
				
				GUILayout.EndHorizontal();
				
				EditorGUILayout.Space();
				
				NGUIEditorTools.DrawSpriteField("Sprite Normal", obj, atlas, obj.FindProperty("mSpriteName"));
				NGUIEditorTools.DrawSpriteField("Sprite Focused", serializedObject, atlas, serializedObject.FindProperty("fieldFocusedSprite"), true);
	
				EditorGUILayout.Space();
				
				NGUIEditorTools.DrawProperty("Color Normal", obj, "mColor");
				NGUIEditorTools.DrawProperty("Color Focused", serializedObject, "fieldFocusedColor");
				
				obj.ApplyModifiedProperties();
			}
			
			NGUIEditorTools.EndContents();
		}
	}
	
	void DrawArrowSprites()
	{
		NGUIEditorTools.SetLabelWidth(80f);
		
		if (NGUIEditorTools.DrawHeader("Arrow Layout"))
		{
			NGUIEditorTools.BeginContents();
			
			NGUIEditorTools.DrawProperty("Arrow Sprite", serializedObject, "arrowElement");
			
			if (mList.arrowElement != null)
			{
				EditorGUILayout.Space();
				
				SerializedObject obj = new SerializedObject(mList.arrowElement);
				obj.Update();
				
				GUILayout.BeginHorizontal();
				
				if (NGUIEditorTools.DrawPrefixButton("Atlas"))
					ComponentSelector.Show<UIAtlas>(OnSelectArrowAtlas);
				
				SerializedProperty atlas = NGUIEditorTools.DrawProperty("", obj, "mAtlas", GUILayout.MinWidth(20f));

				GUILayout.EndHorizontal();
				
				if (mList.arrowElement.atlas != null)
				{
					EditorGUILayout.Space();
					
					NGUIEditorTools.DrawSpriteField("Closed", obj, atlas, obj.FindProperty("mSpriteName"), true);
					NGUIEditorTools.DrawSpriteField("Closed Hover", serializedObject, atlas, serializedObject.FindProperty("arrowClosedHoverSprite"), true);
					
					EditorGUILayout.Space();
					
					NGUIEditorTools.DrawSpriteField("Opened", serializedObject, atlas, serializedObject.FindProperty("arrowOpenedSprite"), true);
					NGUIEditorTools.DrawSpriteField("Opened Hover", serializedObject, atlas, serializedObject.FindProperty("arrowOpenedHoverSprite"), true);
				}
				
				obj.ApplyModifiedProperties();
			}
			
			NGUIEditorTools.EndContents();
		}
	}

	void DrawListLayout()
	{
		NGUIEditorTools.SetLabelWidth(80f);
		
		if (NGUIEditorTools.DrawHeader("List Layout"))
		{
			NGUIEditorTools.BeginContents();
			
			GUILayout.BeginHorizontal();
			
			if (NGUIEditorTools.DrawPrefixButton("Atlas"))
				ComponentSelector.Show<UIAtlas>(OnSelectAtlas);
			
			NGUIEditorTools.DrawProperty("", serializedObject, "atlas", GUILayout.MinWidth(20f));
			
			GUILayout.EndHorizontal();
			
			if (mList.atlas != null)
			{
				EditorGUILayout.Space();
				
				NGUIEditorTools.DrawSpriteField("Background", serializedObject, serializedObject.FindProperty("atlas"), serializedObject.FindProperty("listBackgroundSprite"), true);
				NGUIEditorTools.DrawProperty("BG Color", serializedObject, "listBackgroundColor");
	
				EditorGUILayout.Space();
	
				NGUIEditorTools.DrawProperty("Offset", serializedObject, "listOffset");
	
				EditorGUILayout.Space();
				GUI.changed = false;
	
				NGUIEditorTools.IntVector paddingA = NGUIEditorTools.IntPair("Padding", "Left", "Right", mList.listPaddingLeft, mList.listPaddingRight);
				NGUIEditorTools.IntVector paddingB = NGUIEditorTools.IntPair(null, "Bottom", "Top", mList.listPaddingBottom, mList.listPaddingTop);
	
				if (GUI.changed)
				{
					NGUIEditorTools.RegisterUndo("Select Field Padding Changed", mList);
					
					mList.listPaddingLeft = paddingA.x;
					mList.listPaddingRight = paddingA.y;
					mList.listPaddingBottom = paddingB.x;
					mList.listPaddingTop = paddingB.y;
				}
	
				EditorGUILayout.Space();
	
				NGUIEditorTools.DrawProperty("Add to Size", serializedObject, "listAdditionalSize");
	
				EditorGUILayout.Space();
	
				NGUIEditorTools.DrawSpriteField("Separator", serializedObject, serializedObject.FindProperty("atlas"), serializedObject.FindProperty("listSeparatorSprite"), true);
				
				EditorGUILayout.Space();
				GUI.changed = false;
				
				NGUIEditorTools.IntVector sepMarginA = NGUIEditorTools.IntPair("Sep margin", "Left", "Right", mList.listSepMarginLeft, mList.listSepMarginRight);
				NGUIEditorTools.IntVector sepMarginB = NGUIEditorTools.IntPair(null, "Bottom", "Top", mList.listSepMarginBottom, mList.listSepMarginTop);
				
				if (GUI.changed)
				{
					NGUIEditorTools.RegisterUndo("Select Field Separator Margin Changed", mList);
					
					mList.listSepMarginLeft = sepMarginA.x;
					mList.listSepMarginRight = sepMarginA.y;
					mList.listSepMarginBottom = sepMarginB.x;
					mList.listSepMarginTop = sepMarginB.y;
				}
			}

			EditorGUILayout.Space();

			NGUIEditorTools.DrawProperty("Animation", serializedObject, "listAnimation");
			NGUIEditorTools.DrawProperty("An. Duration", serializedObject, "listAnimationDuration");

			NGUIEditorTools.EndContents();
		}
	}

	void DrawListHighlightLayout()
	{
		if (mList.atlas != null)
		{
			if (NGUIEditorTools.DrawHeader("List Highlight Layout"))
			{
				NGUIEditorTools.BeginContents();
				NGUIEditorTools.SetLabelWidth(80f);
	
				NGUIEditorTools.DrawSpriteField("HL. Sprite", serializedObject, serializedObject.FindProperty("atlas"), serializedObject.FindProperty("listHighlightSprite"), true);
				NGUIEditorTools.DrawProperty("HL. Color", serializedObject, "listHighlightColor");
	
				EditorGUILayout.Space();
				NGUIEditorTools.SetLabelWidth(100f);
				
				NGUIEditorTools.DrawProperty("HL. Label Color", serializedObject, "listHighlightLabelColor");
	
				EditorGUILayout.Space();
				GUI.changed = false;
				
				NGUIEditorTools.IntVector HLMarginA = NGUIEditorTools.IntPair("HL. Margin", "Left", "Right", mList.listHLMarginLeft, mList.listHLMarginRight);
				NGUIEditorTools.IntVector HLMarginB = NGUIEditorTools.IntPair(null, "Bottom", "Top", mList.listHLMarginBottom, mList.listHLMarginTop);
				
				if (GUI.changed)
				{
					NGUIEditorTools.RegisterUndo("Select Field Highlight Margin Changed", mList);
					
					mList.listHLMarginLeft = HLMarginA.x;
					mList.listHLMarginRight = HLMarginA.y;
					mList.listHLMarginBottom = HLMarginB.x;
					mList.listHLMarginTop = HLMarginB.y;
				}
	
				EditorGUILayout.Space();
				NGUIEditorTools.SetLabelWidth(120f);
	
				NGUIEditorTools.DrawProperty("Animate Position", serializedObject, "animateHighlightPosition");
				NGUIEditorTools.DrawProperty("Animate Label Color", serializedObject, "animateHighlightLabel");
				NGUIEditorTools.DrawProperty("Anim. Duration", serializedObject, "animateHighlightDuration");
	
				NGUIEditorTools.EndContents();
			}
		}
	}

	void DrawFont()
	{
		NGUIEditorTools.SetLabelWidth(80f);

		if (NGUIEditorTools.DrawHeader("List Item Layout"))
		{
			NGUIEditorTools.BeginContents();
			
			SerializedProperty ttf = null;
			
			GUILayout.BeginHorizontal();
			{
				if (NGUIEditorTools.DrawPrefixButton("Font"))
				{
					if (mType == FontType.Bitmap)
					{
						ComponentSelector.Show<UIFont>(OnBitmapFont);
					}
					else
					{
						ComponentSelector.Show<Font>(OnDynamicFont, new string[] { ".ttf", ".otf"});
					}
				}
				
				#if DYNAMIC_FONT
				GUI.changed = false;
				mType = (FontType)EditorGUILayout.EnumPopup(mType, GUILayout.Width(62f));
				
				if (GUI.changed)
				{
					GUI.changed = false;
					
					if (mType == FontType.Bitmap)
					{
						serializedObject.FindProperty("trueTypeFont").objectReferenceValue = null;
					}
					else
					{
						serializedObject.FindProperty("bitmapFont").objectReferenceValue = null;
					}
				}
				#else
				mType = FontType.Bitmap;
				#endif
				
				if (mType == FontType.Bitmap)
				{
					NGUIEditorTools.DrawProperty("", serializedObject, "bitmapFont", GUILayout.MinWidth(40f));
				}
				else
				{
					ttf = NGUIEditorTools.DrawProperty("", serializedObject, "trueTypeFont", GUILayout.MinWidth(40f));
				}
			}
			GUILayout.EndHorizontal();
			
			if (ttf != null && ttf.objectReferenceValue != null)
			{
				GUILayout.BeginHorizontal();
				{
					EditorGUI.BeginDisabledGroup(ttf.hasMultipleDifferentValues);
					NGUIEditorTools.DrawProperty("Font Size", serializedObject, "fontSize", GUILayout.Width(142f));
					NGUIEditorTools.DrawProperty("", serializedObject, "fontStyle", GUILayout.MinWidth(40f));
					GUILayout.Space(18f);
					EditorGUI.EndDisabledGroup();
				}
				GUILayout.EndHorizontal();
			}
			else NGUIEditorTools.DrawProperty("Font Size", serializedObject, "fontSize", GUILayout.Width(142f));
			
			NGUIEditorTools.DrawProperty("Text Color", serializedObject, "textColor");
			
			GUILayout.BeginHorizontal();
			NGUIEditorTools.SetLabelWidth(66f);
			EditorGUILayout.PrefixLabel("Padding");
			NGUIEditorTools.SetLabelWidth(14f);
			NGUIEditorTools.DrawProperty("X", serializedObject, "padding.x", GUILayout.MinWidth(30f));
			NGUIEditorTools.DrawProperty("Y", serializedObject, "padding.y", GUILayout.MinWidth(30f));
			GUILayout.Space(18f);
			NGUIEditorTools.SetLabelWidth(80f);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			SerializedProperty sp = NGUIEditorTools.DrawProperty("Effect", serializedObject, "labelEffectStyle", GUILayout.MinWidth(170f));
			GUILayout.Space(18f);
			GUILayout.EndHorizontal();
			
			if (sp.hasMultipleDifferentValues || sp.boolValue)
				NGUIEditorTools.DrawProperty("Effect Color", serializedObject, "labelEffectColor", GUILayout.MinWidth(30f));
			
			if (sp.hasMultipleDifferentValues || sp.boolValue)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label("Distance", GUILayout.Width(56f));
				NGUIEditorTools.SetLabelWidth(20f);
				NGUIEditorTools.DrawProperty("X", serializedObject, "labelEffectDistance.x", GUILayout.MinWidth(40f));
				NGUIEditorTools.DrawProperty("Y", serializedObject, "labelEffectDistance.y", GUILayout.MinWidth(40f));
				GUILayout.Space(18f);
				NGUIEditorTools.SetLabelWidth(80f);
				GUILayout.EndHorizontal();
			}

			NGUIEditorTools.EndContents();
		}
	}

	void RegisterUndo()
	{
		NGUIEditorTools.RegisterUndo("Select Field Change", mList);
	}
	
	void OnSelectAtlas(Object obj)
	{
		RegisterUndo();
		mList.atlas = obj as UIAtlas;
		NGUISettings.atlas = mList.atlas;
	}
	
	void OnSelectBackgroundAtlas(Object obj)
	{
		if (mList.backgroundElement != null)
		{
			RegisterUndo();
			mList.backgroundElement.atlas = obj as UIAtlas;
			NGUISettings.atlas = mList.backgroundElement.atlas;
		}
	}
	
	void OnSelectArrowAtlas(Object obj)
	{
		if (mList.arrowElement != null)
		{
			RegisterUndo();
			mList.arrowElement.atlas = obj as UIAtlas;
			NGUISettings.atlas = mList.arrowElement.atlas;
		}
	}
	
	void OnBitmapFont(Object obj)
	{
		serializedObject.Update();
		SerializedProperty sp = serializedObject.FindProperty("bitmapFont");
		sp.objectReferenceValue = obj;
		serializedObject.ApplyModifiedProperties();
		NGUISettings.ambigiousFont = obj;
	}
	
	void OnDynamicFont(Object obj)
	{
		serializedObject.Update();
		SerializedProperty sp = serializedObject.FindProperty("trueTypeFont");
		sp.objectReferenceValue = obj;
		serializedObject.ApplyModifiedProperties();
		NGUISettings.ambigiousFont = obj;
	}
}
