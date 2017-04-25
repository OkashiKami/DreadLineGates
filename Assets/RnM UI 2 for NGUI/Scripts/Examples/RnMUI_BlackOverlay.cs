using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RnMUI_BlackOverlay : MonoBehaviour {
	
	/// <summary>
	/// The panel holding the overlay.
	/// </summary>
	public UIPanel panel;
	
	/// <summary>
	/// A list with window IDs that should have overlay.
	/// </summary>
	public List<int> overlayWindows = new List<int>()
	{
		WindowIDs.Settings,
		WindowIDs.GameMenu,
	};
	
	/// <summary>
	/// Gets a value indicating whether the overlay panel is visible.
	/// </summary>
	/// <value><c>true</c> if this instance is visible; otherwise, <c>false</c>.</value>
	public bool IsVisible {
		get {
			return ((this.panel != null) ? (this.panel.alpha > 0f) : false);
		}
	}
	
	/// <summary>
	/// Show the overlay.
	/// </summary>
	public void Show()
	{
		if (this.panel == null || this.IsVisible)
			return;
		
		// Bring the overlay panel behind the window
		if (UIWindow.current != null)
		{
			NGUITools.BringForward(this.gameObject);
			NGUITools.BringForward(UIWindow.current.gameObject);
		}
		
		// If this was invoked by a window event and that window uses fading
		if (UIWindow.current != null && UIWindow.current.fading)
		{
			// Use the same fade effect as the window
			TweenAlpha.Begin(this.panel.gameObject, UIWindow.current.fadeDuration, 1f);
		}
		else
		{
			this.panel.alpha = 1f;
		}
	}
	
	/// <summary>
	/// Hide the overlay.
	/// </summary>
	public void Hide()
	{
		if (this.panel == null || !this.IsVisible)
			return;
		
		// Make sure we dont hide the overlay untill all the opened windows that should have a overlay are closed
		foreach (int wID in this.overlayWindows)
		{
			// Check if this is the window being closed
			if (UIWindow.current != null && wID == UIWindow.current.WindowId)
				continue;
			
			// Get the window
			UIWindow w = UIWindow.GetWindow(wID);
			
			// Check if we have the window and that window is open
			// In that case we have to leave the overlay visible
			if (w != null && w.IsOpen)
				return;
		}
		
		// If this was invoked by a window event and that window uses fading
		if (UIWindow.current != null && UIWindow.current.fading)
		{
			// Use the same fade effect as the window
			TweenAlpha.Begin(this.panel.gameObject, UIWindow.current.fadeDuration, 0f);
		}
		else
		{
			this.panel.alpha = 0f;
		}
	}
}
