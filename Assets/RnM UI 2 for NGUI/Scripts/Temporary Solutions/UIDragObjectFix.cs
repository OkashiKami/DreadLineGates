using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UIDragObjectFix : MonoBehaviour {

	[SerializeField] private UIDragObject m_Target;
	
	protected void Start()
	{
		if (this.m_Target == null)
			this.m_Target = this.gameObject.GetComponent<UIDragObject>();
			
		if (this.m_Target != null)
		{
			UIRoot root = NGUITools.FindInParents<UIRoot>(this.transform);
			
			if (root != null)
				this.m_Target.panelRegion = root.GetComponent<UIPanel>();
		}
	}
}
