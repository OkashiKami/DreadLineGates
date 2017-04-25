using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UIToggle))]
[AddComponentMenu("RPG and MMO UI/Tab Button")]
public class RnMUI_Tab : MonoBehaviour {

	public UILabel tabLabel;
	public UISprite tabSprite;

	public GameObject targetContent;
	public RnMUI_Tab linkWith;

	private UIToggle toggle;
	private bool currentState = false;

	public Color inactiveLabelColor = Color.white;
	public Color activeLabelColor = Color.white;
	public Color hoverLabelColor = Color.white;
	public float labelTweenDuration = 0.2f;

	public Color normalSpriteColor = Color.white;
	public Color hoverSpriteColor = Color.white;
	public Color activeSpriteColor = Color.white;
	public float spriteTweenDuration = 0.2f;
	
	/// <summary>
	/// Gets a value indicating whether this tab is active.
	/// </summary>
	/// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
	public bool IsActive
	{
		get {
			return (this.toggle != null ? this.toggle.value : false);
		}
	}
	
	void Start()
	{
		this.toggle = this.GetComponent<UIToggle>();

		if (this.tabLabel != null)
			this.inactiveLabelColor = this.tabLabel.color;

		if (this.toggle == null)
			Debug.LogWarning(this.GetType() + " requires that you define UIToggle in order to work.", this);

		if (this.targetContent == null)
			Debug.LogWarning(this.GetType() + " requires that you define target GameObject to toggle.", this);

		if (this.tabSprite != null)
			this.normalSpriteColor = this.tabSprite.color;

		// Trigger on change just in case the NGUI call fails
		this.OnChange();

		// Hook the on change event
		this.toggle.onChange.Add(new EventDelegate(OnChange));
	}

	private void OnChange()
	{
		// Check if this tab is linked to another
		if (this.linkWith != null)
			this.linkWith.SetState(this.toggle.value);

		// Handle state change
		this.SetState(this.toggle.value);
	}

	void OnHover(bool isOver)
	{
		if (isOver)
		{
			if (this.tabLabel != null)
				TweenColor.Begin(this.tabLabel.gameObject, this.labelTweenDuration, this.hoverLabelColor).method = UITweener.Method.EaseInOut;

			if (this.tabSprite != null)
				TweenColor.Begin(this.tabSprite.gameObject, this.spriteTweenDuration, this.hoverSpriteColor).method = UITweener.Method.EaseInOut;
		}
		else
		{
			if (this.tabLabel != null)
				TweenColor.Begin(this.tabLabel.gameObject, this.labelTweenDuration, (this.currentState) ? this.activeLabelColor : this.inactiveLabelColor).method = UITweener.Method.EaseInOut;

			if (this.tabSprite != null)
				TweenColor.Begin(this.tabSprite.gameObject, this.spriteTweenDuration, (this.currentState) ? this.activeSpriteColor : this.normalSpriteColor).method = UITweener.Method.EaseInOut;
		}
	}
	
	/// <summary>
	/// Sets the state of the tab.
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	public void SetState(bool state)
	{
		// Force the state on the toggle if necessary
		if (this.toggle != null && this.toggle.value != state)
			this.toggle.value = state;

		// Handle state change
		if (state)
		{
			if (this.targetContent != null)
				this.targetContent.SetActive(true);

			// Nozmalize the panels depths
			NGUITools.NormalizePanelDepths();

			if (this.tabLabel != null)
				TweenColor.Begin(this.tabLabel.gameObject, this.labelTweenDuration, this.activeLabelColor).method = UITweener.Method.EaseInOut;
				
			if (this.tabSprite != null)
				TweenColor.Begin(this.tabSprite.gameObject, this.spriteTweenDuration, this.activeSpriteColor).method = UITweener.Method.EaseInOut;
		}
		else
		{
			if (this.targetContent != null)
				this.targetContent.SetActive(false);

			if (this.tabLabel != null)
				TweenColor.Begin(this.tabLabel.gameObject, this.labelTweenDuration, this.inactiveLabelColor).method = UITweener.Method.EaseInOut;
				
			if (this.tabSprite != null)
				TweenColor.Begin(this.tabSprite.gameObject, this.spriteTweenDuration, this.normalSpriteColor).method = UITweener.Method.EaseInOut;
		}

		// Disable or enable the collider
		if (this.GetComponent<Collider>() != null)
			this.GetComponent<Collider>().enabled = !state;

		// Save the state
		this.currentState = state;
	}
	
	/// <summary>
	/// Activates this tab.
	/// </summary>
	public void Activate()
	{
		this.SetState(true);
	}
}
