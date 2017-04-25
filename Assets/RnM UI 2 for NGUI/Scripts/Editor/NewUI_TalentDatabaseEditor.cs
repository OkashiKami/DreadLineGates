using UnityEngine;
using UnityEditor;
using System.Collections;

public class NewUI_TalentDatabaseEditor
{
	private static string GetSelectionFolder()
	{
		if (Selection.activeObject != null)
		{
			string path = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
			
			if (!string.IsNullOrEmpty(path))
			{
				int dot = path.LastIndexOf('.');
				int slash = Mathf.Max(path.LastIndexOf('/'), path.LastIndexOf('\\'));
				if (slash > 0) return (dot > slash) ? path.Substring(0, slash + 1) : path + "/";
			}
		}
		return "Assets/";
	}
	
	[MenuItem("New UI/Create/Talent Database")]
	public static void CreateDatabase()
	{
		// Get the currently selected asset directory
		string currentPath = GetSelectionFolder();
		
		// New asset name
		string assetName = "New Talent Database.asset";
		
		NewUI_TalentDatabase asset = ScriptableObject.CreateInstance("NewUI_TalentDatabase") as NewUI_TalentDatabase;  //scriptable object
		AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath(currentPath + assetName));
		AssetDatabase.Refresh();
	}
}