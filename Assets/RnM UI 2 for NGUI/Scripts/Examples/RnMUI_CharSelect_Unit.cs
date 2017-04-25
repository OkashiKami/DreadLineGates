using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIToggle))]
public class RnMUI_CharSelect_Unit : MonoBehaviour {
	
	private UIToggle toggle;
	
	public UILabel nameLabel;
	private Color nameLebelNormalColor = Color.white;
	public Color nameLabelActiveColor = Color.white;
	
	public UILabel preLevelLabel;
	public UILabel levelLabel;
	private Color levelLabelNormalColor = Color.white;
	public Color levelLabelActiveColor = Color.white;
	
	public UILabel raceLabel;
	private Color raceLabelNormalColor = Color.white;
	public Color raceLabelActiveColor = Color.white;
	
	public UILabel classLabel;
	private Color classLabelNormalColor = Color.white;
	public Color classLabelActiveColor = Color.white;
	
	public bool animateLabels = true;
	public float labelsTweenDuration = 0.15f;
	
	public UISprite hoverSprite;
	private Color hoverSpriteNormalColor = Color.white;
	public Color hoverSpriteHoverColor = Color.white;
	public float hoverTweenDuration = 0.15f;
	
	public UIButton deleteBtn;
	
	void Start()
	{
		// Get the Toggle
		this.toggle = this.GetComponent<UIToggle>();
		
		// Get the labels normal colours
		if (this.nameLabel != null)
			this.nameLebelNormalColor = this.nameLabel.color;
			
		if (this.levelLabel != null)
			this.levelLabelNormalColor = this.levelLabel.color;
			
		if (this.raceLabel != null)
			this.raceLabelNormalColor = this.raceLabel.color;
			
		if (this.classLabel != null)
			this.classLabelNormalColor = this.classLabel.color;
		
		// Get the hover sprite normal color
		if (this.hoverSprite != null)
			this.hoverSpriteNormalColor = this.hoverSprite.color;
		
		// Trigger on change just in case the NGUI call fails
		this.OnChange();
		
		// Hook the on change event
		this.toggle.onChange.Add(new EventDelegate(OnChange));
		
		// Hook the delete button hover event
		if (this.deleteBtn != null)
			UIEventListener.Get(this.deleteBtn.gameObject).onHover += OnHoverProxy;
	}
	
	protected void OnHoverProxy(GameObject go, bool state)
	{
		this.OnHover(state);
	}
	
	protected void OnHover(bool isOver)
	{
		if (isOver)
		{
			if (this.hoverSprite != null)
				TweenColor.Begin(this.hoverSprite.gameObject, this.hoverTweenDuration, this.hoverSpriteHoverColor).method = UITweener.Method.EaseInOut;
		}
		else
		{
			if (this.hoverSprite != null)
				TweenColor.Begin(this.hoverSprite.gameObject, this.hoverTweenDuration, this.hoverSpriteNormalColor).method = UITweener.Method.EaseInOut;
		}
	}
	
	private void OnChange()
	{
		// Handle state change
		this.SetState(this.toggle.value);
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
		if (this.animateLabels)
		{
			if (this.nameLabel != null) TweenColor.Begin(this.nameLabel.gameObject, this.labelsTweenDuration, (state ? this.nameLabelActiveColor : this.nameLebelNormalColor)).method = UITweener.Method.EaseInOut;
			if (this.preLevelLabel != null) TweenColor.Begin(this.preLevelLabel.gameObject, this.labelsTweenDuration, (state ? this.levelLabelActiveColor : this.levelLabelNormalColor)).method = UITweener.Method.EaseInOut;
			if (this.levelLabel != null) TweenColor.Begin(this.levelLabel.gameObject, this.labelsTweenDuration, (state ? this.levelLabelActiveColor : this.levelLabelNormalColor)).method = UITweener.Method.EaseInOut;
			if (this.raceLabel != null) TweenColor.Begin(this.raceLabel.gameObject, this.labelsTweenDuration, (state ? this.raceLabelActiveColor : this.raceLabelNormalColor)).method = UITweener.Method.EaseInOut;
			if (this.classLabel != null) TweenColor.Begin(this.classLabel.gameObject, this.labelsTweenDuration, (state ? this.classLabelActiveColor : this.classLabelNormalColor)).method = UITweener.Method.EaseInOut;
		}
		else
		{
			if (this.nameLabel != null) this.nameLabel.color = (state ? this.nameLabelActiveColor : this.nameLebelNormalColor);
			if (this.preLevelLabel != null) this.preLevelLabel.color = (state ? this.levelLabelActiveColor : this.levelLabelNormalColor);
			if (this.levelLabel != null) this.levelLabel.color = (state ? this.levelLabelActiveColor : this.levelLabelNormalColor);
			if (this.raceLabel != null) this.raceLabel.color = (state ? this.raceLabelActiveColor : this.raceLabelNormalColor);
			if (this.classLabel != null) this.classLabel.color = (state ? this.classLabelActiveColor : this.classLabelNormalColor);
		}
	}
	
	/// <summary>
	/// Activates this tab.
	/// </summary>
	public void Activate()
	{
		this.SetState(true);
	}
}
