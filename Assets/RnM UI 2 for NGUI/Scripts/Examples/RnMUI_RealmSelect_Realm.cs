using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIToggle))]
public class RnMUI_RealmSelect_Realm : MonoBehaviour {
	
	public enum Status
	{
		Online,
		Offline,
	}
	
	public enum Population
	{
		Low,
		Medium,
		High,
	}
	
	public UISprite target;
	private UIToggle toggle;
	
	public bool isClosed = false;
	public Status currentStatus = Status.Offline;
	public Population currentPop = Population.Low;
	
	private string normalSprite;
	public string hoverSprite;
	public string activeSprite;
	
	public UILabel nameLabel;
	private Color nameNormalColor = Color.white;
	public Color nameHoverColor = Color.white;
	public Color nameClosedColor = Color.gray;
	
	public UILabel statusLabel;
	public Color statusOnlineColor = Color.green;
	public Color statusOfflineColor = Color.red;
	
	public UILabel populationLabel;
	public Color popLowColor = Color.green;
	public Color popMediumColor = Color.yellow;
	public Color popHighColor = Color.red;
	
	public UILabel charsLabel;
	
	public UILabel closedLabel;
	private Color closedNormalColor = Color.white;
	public Color closedHoverColor = Color.white;
	
	void Start()
	{
		// Grab the toggle
		this.toggle = this.GetComponent<UIToggle>();
		
		// Get the normal sprite name
		if (this.target != null)
			this.normalSprite = this.target.spriteName;
		
		// Get the name label normal color
		if (this.nameLabel != null)
			this.nameNormalColor = this.nameLabel.color;
		
		// Get the closed label normal color
		if (this.closedLabel != null)
			this.closedNormalColor = this.closedLabel.color;
		
		// Trigger on change just in case the NGUI call fails
		this.OnChange();
		
		// Hook the on change event
		this.toggle.onChange.Add(new EventDelegate(OnChange));
		
		// Set the starting closed state
		this.SetClosed(this.isClosed);
		
		// Set the starting status state
		this.SetStatus(this.currentStatus);
		
		// Set the starting population state
		this.SetPopulation(this.currentPop);
	}
	
	void OnHover(bool isOver)
	{
		if (this.target == null)
			return;
		
		// Set the target sprite
		if (!this.toggle.value)
			this.target.spriteName = ((isOver && !string.IsNullOrEmpty(this.hoverSprite)) ? this.hoverSprite : this.normalSprite);
		
		// Update the name label color
		if (this.nameLabel != null)
			this.nameLabel.color = (isOver ? this.nameHoverColor : (this.isClosed ? this.nameClosedColor : this.nameNormalColor));
			
		// Update the closed label color
		if (this.closedLabel != null)
			this.closedLabel.color = (isOver ? this.closedHoverColor : this.closedNormalColor);
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
		if (state)
		{
			if (this.target != null)
				this.target.spriteName = this.activeSprite;
		}
		else
		{
			if (this.target != null)
				this.target.spriteName = (UICamera.IsHighlighted(this.gameObject) ? this.hoverSprite : this.normalSprite);
		}
	}
	
	/// <summary>
	/// Sets the status state.
	/// </summary>
	/// <param name="status">The status state.</param>
	public void SetStatus(Status status)
	{
		// Save the status state
		this.currentStatus = status;
		
		// Update the label color and text
		if (this.statusLabel != null)
		{
			switch (status)
			{
			case Status.Offline:
				this.statusLabel.text = "Offline";
				this.statusLabel.color = this.statusOfflineColor;
				break;
			case Status.Online:
				this.statusLabel.text = "Online";
				this.statusLabel.color = this.statusOnlineColor;
				break;
			}
		}
	}
	
	/// <summary>
	/// Sets the population state.
	/// </summary>
	/// <param name="pop">The population state.</param>
	public void SetPopulation(Population pop)
	{
		// Save the state
		this.currentPop = pop;
		
		// Update the label color and text
		if (this.populationLabel != null)
		{
			switch (pop)
			{
			case Population.Low:
				this.populationLabel.text = "Low";
				this.populationLabel.color = this.popLowColor;
				break;
			case Population.Medium:
				this.populationLabel.text = "Medium";
				this.populationLabel.color = this.popMediumColor;
				break;
			case Population.High:
				this.populationLabel.text = "High";
				this.populationLabel.color = this.popHighColor;
				break;
			}
		}
	}
	
	/// <summary>
	/// Sets the closed state.
	/// </summary>
	/// <param name="state">If set to <c>true</c> it's closed.</param>
	public void SetClosed(bool state)
	{
		// Enable or disable the toggle
		this.toggle.enabled = !state;
		
		// Save the state
		this.isClosed = state;
		
		// Update the name label color
		if (this.nameLabel != null)
			this.nameLabel.color = (state ? this.nameClosedColor : this.nameNormalColor);
			
		// Enable or disable the closed label
		if (this.closedLabel != null)
			this.closedLabel.enabled = this.isClosed;
	}
	
	/// <summary>
	/// Sets the name of the realm.
	/// </summary>
	/// <param name="name">Name.</param>
	public void SetRealmName(string name)
	{
		if (this.nameLabel != null)
			this.nameLabel.text = name;
	}
	
	/// <summary>
	/// Sets the characters count.
	/// </summary>
	/// <param name="count">Count.</param>
	public void SetCharactersCount(int count)
	{
		if (this.charsLabel != null)
			this.charsLabel.text = count.ToString();
	}
	
	/// <summary>
	/// Activates this tab.
	/// </summary>
	public void Activate()
	{
		this.SetState(true);
	}
}
