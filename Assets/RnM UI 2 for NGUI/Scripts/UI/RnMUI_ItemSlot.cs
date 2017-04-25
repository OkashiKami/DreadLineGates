using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("RPG and MMO UI/Item Slot")]
public class RnMUI_ItemSlot : RnMUI_IconSlot
{
	private UIItemInfo itemInfo;
	
	/// <summary>
	/// Gets the ItemInfo of the item assigned to this slot.
	/// </summary>
	/// <returns>The item info.</returns>
	public UIItemInfo GetItemInfo()
	{
		return itemInfo;
	}
	
	/// <summary>
	/// Determines whether this slot is assigned.
	/// </summary>
	/// <returns><c>true</c> if this instance is assigned; otherwise, <c>false</c>.</returns>
	public override bool IsAssigned()
	{
		return (this.itemInfo != null);
	}
	
	/// <summary>
	/// Assign the slot by item info.
	/// </summary>
	/// <param name="itemInfo">Item info.</param>
	public bool Assign(UIItemInfo itemInfo)
	{
		if (itemInfo == null)
			return false;
		
		// Use the base class assign
		base.Assign(itemInfo.Icon);
		
		// Set the item info
		this.itemInfo = itemInfo;

		// Success
		return true;
	}
	
	/// <summary>
	/// Assign the slot by the passed source slot.
	/// </summary>
	/// <param name="source">Source.</param>
	public override bool Assign(Object source)
	{
		if (source is RnMUI_ItemSlot)
		{
			RnMUI_ItemSlot sourceSlot = source as RnMUI_ItemSlot;
			
			if (sourceSlot != null)
				return this.Assign(sourceSlot.GetItemInfo());
		}
		else if (source is RnMUI_EquipSlot)
		{
			RnMUI_EquipSlot sourceSlot = source as RnMUI_EquipSlot;
			
			if (sourceSlot != null)
				return this.Assign(sourceSlot.GetItemInfo());
		}
		
		// Default
		return false;
	}
	
	/// <summary>
	/// Unassign this slot.
	/// </summary>
	public override void Unassign()
	{
		// Remove the icon
		base.Unassign();
		
		// Clear the item info
		this.itemInfo = null;
	}
	
	/// <summary>
	/// Determines whether this slot can swap with the specified target slot.
	/// </summary>
	/// <returns><c>true</c> if this instance can swap with the specified target; otherwise, <c>false</c>.</returns>
	/// <param name="target">Target.</param>
	public override bool CanSwapWith(Object target)
	{
		if ((target is RnMUI_ItemSlot) || (target is RnMUI_EquipSlot))
		{
			// Check if the equip slot accpets this item
			if (target is RnMUI_EquipSlot)
			{
				return (target as RnMUI_EquipSlot).CheckEquipType(this.GetItemInfo());
			}
			
			// It's an item slot
			return true;
		}
		
		return false;
	}
	
	/// <summary>
	/// Performs a slot swap.
	/// </summary>
	/// <param name="targetObject">Target slot.</param>
	public override bool PerformSlotSwap(Object targetObject)
	{
		UIItemInfo targetItemInfo = null;
		bool assign1 = false;
		
		if (targetObject is RnMUI_ItemSlot)
		{
			targetItemInfo = (targetObject as RnMUI_ItemSlot).GetItemInfo();
			assign1 = (targetObject as RnMUI_ItemSlot).Assign(this);
		}
		else if (targetObject is RnMUI_EquipSlot)
		{
			targetItemInfo = (targetObject as RnMUI_EquipSlot).GetItemInfo();
			assign1 = (targetObject as RnMUI_EquipSlot).Assign(this);
		}	
		
		// Assign this slot by the target slot item info
		bool assign2 = this.Assign(targetItemInfo);
		
		// Return the status
		return (assign1 && assign2);
	}
	
	/// <summary>
	/// Handles the drop on custom surface.
	/// </summary>
	/// <returns><c>true</c>, if custom surface was handled, <c>false</c> otherwise.</returns>
	/// <param name="surface">Surface.</param>
	protected override bool HandleCustomSurface(GameObject surface)
	{
		// Get the quip receiver
		RnMUI_EquipReceiver EquipReceiver = surface.GetComponent<RnMUI_EquipReceiver>();
		
		if (EquipReceiver != null)
		{
			// Try finding a suitable slot for the quip
			RnMUI_EquipSlot targetSlot = EquipReceiver.GetSlotByType(this.GetItemInfo().EquipType);
			
			// Check if the slot was found
			if (targetSlot != null)
			{
				// Normal empty slot assignment
				if (!targetSlot.IsAssigned())
				{
					// Assign the target slot with the info from this one
					if (targetSlot.Assign(this))
						this.Unassign();
				}
				// The target slot is assigned
				else
				{
					// Check if we can swap
					if (this.CanSwapWith(targetSlot))
					{
						// Swap the slots
						this.PerformSlotSwap(targetSlot);
					}
				}
			}
			
			// Return custom surface handled
			return true;
		}
		
		// No custom surface was handled
		return false;
	}
	
	/// <summary>
	/// Raises the click event.
	/// </summary>
	public override void OnClick()
	{
		if (!this.IsAssigned())
			return;
	}

	/// <summary>
	/// Shows a tooltip with the given item info.
	/// </summary>
	/// <param name="info">Info.</param>
	/// <param name="widget">Widget.</param>
	public static void ShowTooltip(UIItemInfo info, UIWidget widget)
	{
		if (info == null)
			return;

		// Set the title and description
		RnMUI_Tooltip.SetTitle(info.Name);
		RnMUI_Tooltip.SetDescription(info.Description);
		
		// Item types
		RnMUI_Tooltip.AddAttribute(info.Type, "", new RectOffset(0, 0, 0, 3));
		RnMUI_Tooltip.AddAttribute(info.Subtype, "", new RectOffset(0, 0, 0, 3));
		
		if (info.ItemType == 1)
		{
			RnMUI_Tooltip.AddAttribute(info.Damage.ToString(), " Damage");
			RnMUI_Tooltip.AddAttribute(info.AttackSpeed.ToString("0.0"), " Attack speed");
			
			RnMUI_Tooltip.AddAttribute_SingleColumn("(" + ((float)info.Damage / info.AttackSpeed).ToString("0.0") + " damage per second)", "", new RectOffset(0, 0, 2, 0));
		}
		else
		{
			RnMUI_Tooltip.AddAttribute(info.Block.ToString(), " Block");
			RnMUI_Tooltip.AddAttribute(info.Armor.ToString(), " Armor");
		}
		
		RnMUI_Tooltip.AddAttribute_SingleColumn("", "+" + info.Stamina.ToString() + " Stamina", new RectOffset(0, 0, 7, 0));
		RnMUI_Tooltip.AddAttribute_SingleColumn("", "+" + info.Strength.ToString() + " Strength");
		
		// Set the tooltip position
		RnMUI_Tooltip.SetPosition(widget);
		
		// Show the tooltip
		RnMUI_Tooltip.Show();
	}
	
	/// <summary>
	/// Raises the tooltip event.
	/// </summary>
	/// <param name="show">If set to <c>true</c> show.</param>
	public override void OnTooltip(bool show)
	{
		if (show && this.IsAssigned())
		{
			// Show the tooltip
			RnMUI_ItemSlot.ShowTooltip(this.itemInfo, this.iconSprite as UIWidget);
			
			// Prevent hide
			return;
		}
		
		// Default hide
		RnMUI_Tooltip.Hide();
	}
}

