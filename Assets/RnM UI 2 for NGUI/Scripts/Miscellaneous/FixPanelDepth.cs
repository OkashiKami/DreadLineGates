using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class FixPanelDepth : MonoBehaviour
{
	void Start()
	{
		#if UNITY_EDITOR
		int depth = UIPanel.nextUnusedDepth;
		UIPanel[] panels = this.gameObject.GetComponentsInChildren<UIPanel>();
		foreach (UIPanel p in panels)
				p.depth = depth++;
		#endif
		DestroyImmediate(this); // Remove this script
	}
}