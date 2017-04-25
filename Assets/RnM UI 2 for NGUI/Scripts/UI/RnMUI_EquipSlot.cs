using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("RPG and MMO UI/Equip Slot")]
public class RnMUI_EquipSlot : RnMUI_IconSlot
{
	public UIEquipmentType equipType = UIEquipmentType.None;

	private UIItemInfo itemInfo;
	
	protected override void Start()
	{
		base.Start();
		this.dragAndDropEnabled = true;
		this.AllowThrowAway = false;
	}
	
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
			{
				// Check if the equipment type matches the target slot
				if (!this.CheckEquipType(sourceSlot.GetItemInfo()))
					return false;

				return this.Assign(sourceSlot.GetItemInfo());
			}
		}
		else if (source is RnMUI_EquipSlot)
		{
			RnMUI_EquipSlot sourceSlot = source as RnMUI_EquipSlot;
			
			if (sourceSlot != null)
			{
				// Check if the equipment type matches the target slot
				if (!this.CheckEquipType(sourceSlot.GetItemInfo()))
					return false;

				// Type matches
				return this.Assign(sourceSlot.GetItemInfo());
			}
		}
		
		// Default
		return false;
	}

	/// <summary>
	/// Checks if the given item can assigned to this slot.
	/// </summary>
	/// <returns><c>true</c>, if equip type was checked, <c>false</c> otherwise.</returns>
	/// <param name="info">Info.</param>
	public virtual bool CheckEquipType(UIItemInfo info)
	{
		if (info.EquipType != this.equipType)
			return false;

		return true;
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
	/// This method is raised when the slot is denied to be thrown away and returned to it's source.
	/// </summary>
	protected override void OnThrowAwayDenied()
	{
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
	/// Raises the click event.
	/// </summary>
	public override void OnClick()
	{
		if (!this.IsAssigned())
			return;
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
		else if (show && !this.IsAssigned())
		{
			RnMUI_Tooltip.SetTitle(this.EquipTypeToString());

			// Set the tooltip position
			RnMUI_Tooltip.SetPosition(this.iconSprite as UIWidget);
			
			// Show the tooltip
			RnMUI_Tooltip.Show();

			// Prevent hide
			return;
		}
		
		// Default hide
		RnMUI_Tooltip.Hide();
	}

	public string EquipTypeToString()
	{
		string str = "Undefined";

		switch (this.equipType)
		{
			case UIEquipmentType.Head: 				str = "Head"; 		break;
			case UIEquipmentType.Necklace:			str = "Necklace"; 	break;
			case UIEquipmentType.Shoulders: 		str = "Shoulders"; 	break;
			case UIEquipmentType.Chest: 			str = "Chest"; 		break;
			case UIEquipmentType.Bracers: 			str = "Bracers"; 	break;
			case UIEquipmentType.Gloves: 			str = "Gloves"; 	break;
			case UIEquipmentType.Belt: 				str = "Belt"; 		break;
			case UIEquipmentType.Pants: 			str = "Pants"; 		break;
			case UIEquipmentType.Boots: 			str = "Boots"; 		break;
			case UIEquipmentType.Trinket: 			str = "Trinket"; 	break;
			case UIEquipmentType.Weapon_MainHand: 	str = "Main Hand"; 	break;
			case UIEquipmentType.Weapon_OffHand: 	str = "Off Hand"; 	break;
		}

		return str;
	}
}

