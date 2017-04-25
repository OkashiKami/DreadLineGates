using UnityEngine;
using System.Collections;

public class RnMUI_EquipReceiver : MonoBehaviour {

	public Transform slotsContainer;
	
	public UILabel hintLabel;
	public bool Animate = true;
	public float animateDuration = 0.5f;
	
	public RnMUI_EquipSlot GetSlotByType(UIEquipmentType type)
	{
		RnMUI_EquipSlot[] slots = this.GetComponentsInChildren<RnMUI_EquipSlot>();

		// Find a suitable slot for the given type
		foreach (RnMUI_EquipSlot slot in slots)
		{
			if (slot.equipType == type)
				return slot;
		}

		return null;
	}
	
	public void OnDragOver(GameObject go)
	{
		if (this.hintLabel == null)
			return;
		
		RnMUI_ItemSlot slot = go.GetComponent<RnMUI_ItemSlot>();
		
		// If we are dragging an item
		if (slot != null)
			this.SetLabelAlpha(1f);
	}
	
	public void OnDragOut(GameObject go)
	{
		if (this.hintLabel == null)
			return;

		this.SetLabelAlpha(0f);
	}
	
	private void SetLabelAlpha(float alpha)
	{
		if (this.Animate)
			TweenAlpha.Begin(this.hintLabel.gameObject, this.animateDuration, alpha);
		else
			this.hintLabel.alpha = alpha;
	}
}
