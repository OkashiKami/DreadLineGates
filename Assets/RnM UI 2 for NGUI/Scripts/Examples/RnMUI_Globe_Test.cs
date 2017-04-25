using UnityEngine;
using System.Collections;

public class RnMUI_Globe_Test : MonoBehaviour {
	
	public UISprite bar;
	public UIEasing.EasingType easing = UIEasing.EasingType.easeLinear;
	public float duration = 2f;
	public float startDelay = 0f;
	public float holdTime = 1f;
	
	public bool startFromZero = false;
	
	private bool started = false;
	private bool ascending = false;
	
	void Start()
	{
		if (this.bar == null)
			this.bar = this.GetComponent<UISprite>();
		
		if (this.startFromZero)
		{
			this.ascending = true;
			this.bar.fillAmount = 0f;
		}
	}
	
	void OnEnable()
	{
		if (this.bar != null)
			this.StartCoroutine("Progress");
	}
	
	private IEnumerator Progress()
	{
		if (!this.started && this.startDelay > 0f)
		{
			this.started = true;
			yield return new WaitForSeconds(this.startDelay);
		}
		
		float startTime = Time.time;
		
		while (Time.time <= (startTime + this.duration))
		{
			float RemainingTime = ((startTime + this.duration) - Time.time);
			float ElapsedTime = (this.duration - RemainingTime);
			
			float eased = UIEasing.Ease(this.easing, ElapsedTime, 0f, 1f, this.duration);
			
			// Invert in case of decending
			if (!this.ascending)
				eased = 1f - eased;
			
			this.bar.fillAmount = eased;
			
			yield return 0;
		}
		
		// Round up the value
		this.bar.fillAmount = (this.ascending) ? 1f : 0f;
		
		if (this.holdTime > 0f)
			yield return new WaitForSeconds(this.holdTime);
		
		this.ascending = !this.ascending;
		this.StartCoroutine("Progress");
	}
}
