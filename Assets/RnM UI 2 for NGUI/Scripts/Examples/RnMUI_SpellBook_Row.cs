using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RnMUI_SpellBook_Row : MonoBehaviour {

	public UILabel rankLabel;
	public UILabel nameLabel;
	public UILabel descriptionLabel;

	public UITableExtended table;
	
	private UIWidget widget;
	public int minimumHeight = 0;
	
	void Start()
	{
		this.widget = this.GetComponent<UIWidget>();
		if (this.widget != null) this.widget.onChange += OnSizeChange;
	}
	
	public void OnAssign()
	{
		RnMUI_SpellSlot slot = RnMUI_SpellSlot.current;

		if (slot != null)
		{
			UISpellInfo info = slot.GetSpellInfo();

			if (info != null)
			{
				if (this.rankLabel != null)
					this.rankLabel.text = Random.Range(1, 5).ToString();

				if (this.nameLabel != null)
					this.nameLabel.text = info.Name;

				if (this.descriptionLabel != null)
					this.descriptionLabel.text = info.Description;
			}
		}
	}

	public void OnUnassign()
	{
		if (this.rankLabel != null)
			this.rankLabel.text = "0";
		
		if (this.nameLabel != null)
			this.nameLabel.text = "";
		
		if (this.descriptionLabel != null)
			this.descriptionLabel.text = "";

		// Update the table layout
		if (this.table != null)
			this.table.Reposition();
	}

	private static UIWindow.UICoroutine svUpdateCoroutine;

	public void OnSizeChange()
	{
		if (this.widget != null && this.widget.height < this.minimumHeight)
			this.widget.height = this.minimumHeight;
	
		// We need to update the parent container anchors before updating the table
		if (this.widget != null)
			this.widget.UpdateAnchors();

		// As we know where the table is, we could try to get it right here
		if (this.table == null)
			this.table = this.transform.parent.GetComponent<UITableExtended>();

		// Update the table layout
		if (this.table != null)
			this.table.Reposition();

		// After the table update we need to update the scroll view
		UIPanel panel = NGUITools.FindInParents<UIPanel>(this.transform);

		if (panel != null)
		{
			UIScrollView sv = panel.GetComponent<UIScrollView>();

			if (sv != null)
			{
				// Not sure why but it seems immidiate update doesnt work,
				// so we'll do one with a 100 ms delay
				// and cancel any coroutines that within the 100 ms timeframe
				if (svUpdateCoroutine != null)
					svUpdateCoroutine.Stop();

				svUpdateCoroutine = new UIWindow.UICoroutine(sv as MonoBehaviour, UpdateScrollView(sv));
			}
		}
	}

	IEnumerator UpdateScrollView(UIScrollView sv)
	{
		yield return new WaitForSeconds(0.1f);

		sv.UpdateScrollbars(true);
	}
}
