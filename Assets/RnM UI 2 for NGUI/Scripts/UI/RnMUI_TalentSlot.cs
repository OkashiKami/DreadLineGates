using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("RPG and MMO UI/Talent Slot")]
public class RnMUI_TalentSlot : RnMUI_IconSlot
{
	public UILabel pointsLabel;
	public Color pointLabelMinColor = Color.white;
	public Color pointLabelMaxColor = Color.white;
	public Color pointLabelActiveColor = Color.white;
	
	private UITalentInfo talentInfo;
	private UISpellInfo spellInfo;
	
	private int currentPoints = 0;
	
	protected override void Start()
	{
		// Disable Drag and Drop
		this.dragAndDropEnabled = false;
	}
	
	/// <summary>
	/// Gets the spell info of the spell assigned to this slot.
	/// </summary>
	/// <returns>The spell info.</returns>
	public UISpellInfo GetSpellInfo()
	{
		return this.spellInfo;
	}
	
	/// <summary>
	/// Gets the talent info of the talent assigned to this slot.
	/// </summary>
	/// <returns>The talent info.</returns>
	public UITalentInfo GetTalentInfo()
	{
		return this.talentInfo;
	}
	
	/// <summary>
	/// Determines whether this slot is assigned.
	/// </summary>
	/// <returns><c>true</c> if this instance is assigned; otherwise, <c>false</c>.</returns>
	public override bool IsAssigned()
	{
		return (this.talentInfo != null);
	}
	
	/// <summary>
	/// Assign the specified slot by talentInfo and spellInfo.
	/// </summary>
	/// <param name="talentInfo">Talent info.</param>
	/// <param name="spellInfo">Spell info.</param>
	public bool Assign(UITalentInfo talentInfo, UISpellInfo spellInfo)
	{
		if (talentInfo == null || spellInfo == null)
			return false;
		
		// Use the base class to assign the icon
		base.Assign(spellInfo.Icon);
		
		// Set the talent info
		this.talentInfo = talentInfo;
			
		// Set the spell info
		this.spellInfo = spellInfo;
		
		// Update the points label
		this.UpdatePointsLabel();
		
		// Return success
		return true;
	}
	
	/// <summary>
	/// Updates the points label.
	/// </summary>
	public void UpdatePointsLabel()
	{
		if (this.pointsLabel == null)
			return;
			
		// Set the points string on the label
		this.pointsLabel.text = "";
		
		// No points assigned
		if (this.currentPoints == 0)
		{
			this.pointsLabel.text += "[" + NGUIText.EncodeColor(this.pointLabelMinColor) + "]" + this.currentPoints.ToString() + "[-]";
			this.pointsLabel.text += "[" + NGUIText.EncodeColor(this.pointLabelMaxColor) + "]/[-]";
			this.pointsLabel.text += "[" + NGUIText.EncodeColor(this.pointLabelMaxColor) + "]" + this.talentInfo.maxPoints.ToString() + "[-]";
		}
		// Assigned but not maxec
		else if (this.currentPoints > 0 && this.currentPoints < this.talentInfo.maxPoints)
		{
			this.pointsLabel.text += "[" + NGUIText.EncodeColor(this.pointLabelActiveColor) + "]" + this.currentPoints.ToString() + "[-]";
			this.pointsLabel.text += "[" + NGUIText.EncodeColor(this.pointLabelMaxColor) + "]/[-]";
			this.pointsLabel.text += "[" + NGUIText.EncodeColor(this.pointLabelMaxColor) + "]" + this.talentInfo.maxPoints.ToString() + "[-]";
		}
		// Maxed
		else
		{
			this.pointsLabel.text += "[" + NGUIText.EncodeColor(this.pointLabelActiveColor) + "]" + this.currentPoints.ToString() + "[-]";
			this.pointsLabel.text += "[" + NGUIText.EncodeColor(this.pointLabelActiveColor) + "]/[-]";
			this.pointsLabel.text += "[" + NGUIText.EncodeColor(this.pointLabelActiveColor) + "]" + this.talentInfo.maxPoints.ToString() + "[-]";
		}
	}
	
	/// <summary>
	/// Unassign this slot.
	/// </summary>
	public override void Unassign()
	{
		// Remove the icon
		base.Unassign();
		
		// Clear the talent info
		this.talentInfo = null;
		
		// Clear the spell info
		this.spellInfo = null;
	}

	/// <summary>
	/// Raises the click event.
	/// </summary>
	public override void OnClick()
	{
		if (!this.IsAssigned())
			return;
		
		// Check for right click
		if (UICamera.currentTouchID == -2)
		{
			this.OnRightClick();
			return;
		}
		
		// Check if the talent is maxed
		if (this.currentPoints >= this.talentInfo.maxPoints)
			return;
		
		// Increase the points
		this.currentPoints = this.currentPoints + 1;
		
		// Update the label string
		this.UpdatePointsLabel();
	}
	
	/// <summary>
	/// Raises the right click event.
	/// </summary>
	public void OnRightClick()
	{
		// Check if the talent is at it's base
		if (this.currentPoints == 0)
			return;
		
		// Increase the points
		this.currentPoints = this.currentPoints - 1;
		
		// Update the label string
		this.UpdatePointsLabel();
	}
	
	/// <summary>
	/// Adds points.
	/// </summary>
	/// <param name="points">Points.</param>
	public void AddPoints(int points)
	{
		if (!this.IsAssigned() || points == 0)
			return;
			
		// Add the points
		this.currentPoints = this.currentPoints + points;
		
		// Make sure we dont exceed the limites
		if (this.currentPoints < 0)
			this.currentPoints = 0;
		
		if (this.currentPoints > this.talentInfo.maxPoints)
			this.currentPoints = this.talentInfo.maxPoints;
			
		// Update the label string
		this.UpdatePointsLabel();
	}
	
	/// <summary>
	/// Raises the tooltip event.
	/// </summary>
	/// <param name="show">If set to <c>true</c> show.</param>
	public override void OnTooltip(bool show)
	{
		if (show && this.IsAssigned())
		{
			// Set the title and description
			RnMUI_Tooltip.SetTitle(this.spellInfo.Name);
			RnMUI_Tooltip.SetDescription(this.spellInfo.Description);
			
			if (this.spellInfo.Flags.Has(UISpellInfo_Flags.Passive))
			{
				RnMUI_Tooltip.AddAttribute("Passive", "");
			}
			else
			{
				// Power consumption
				if (this.spellInfo.PowerCost > 0f)
				{
					if (this.spellInfo.Flags.Has(UISpellInfo_Flags.PowerCostInPct))
						RnMUI_Tooltip.AddAttribute(this.spellInfo.PowerCost.ToString("0") + "%", " Energy");
					else
						RnMUI_Tooltip.AddAttribute(this.spellInfo.PowerCost.ToString("0"), " Energy");
				}
				
				// Range
				if (this.spellInfo.Range > 0f)
				{
					if (this.spellInfo.Range == 1f)
						RnMUI_Tooltip.AddAttribute("Melee range", "");
					else
						RnMUI_Tooltip.AddAttribute(this.spellInfo.Range.ToString("0"), " yd range");
				}
				
				// Cast time
				if (this.spellInfo.CastTime == 0f)
					RnMUI_Tooltip.AddAttribute("Instant", "");
				else
					RnMUI_Tooltip.AddAttribute(this.spellInfo.CastTime.ToString("0.0"), " sec cast");
				
				// Cooldown
				if (this.spellInfo.Cooldown > 0f)
					RnMUI_Tooltip.AddAttribute(this.spellInfo.Cooldown.ToString("0.0"), " sec cooldown");
			}
			
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
}

