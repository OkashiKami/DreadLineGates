﻿using UnityEngine;
using System.Collections;

public class RnMUI_Assign_TalentSlot : MonoBehaviour {
	
	public RnMUI_TalentSlot slot;
	public NewUI_TalentDatabase talentDatabase;
	public NewUI_SpellDatabase spellDatabase;
	public int assignTalent = 0;
	public int addPoints = 0;
	
	void Start()
	{
		if (this.slot == null)
			this.slot = this.GetComponent<RnMUI_TalentSlot>();
		
		if (this.slot == null || this.talentDatabase == null || this.spellDatabase == null)
		{
			this.Destruct();
			return;
		}
		
		UITalentInfo info = this.talentDatabase.GetByID(this.assignTalent);
		
		if (info != null)
		{
			this.slot.Assign(info, this.spellDatabase.GetByID(info.spellEntry));
			this.slot.AddPoints(this.addPoints);
		}
		
		this.Destruct();
	}
	
	private void Destruct()
	{
		DestroyImmediate(this);
	}
}
