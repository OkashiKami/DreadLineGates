using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("RPG and MMO UI/Slot Cooldown")]
public class RnMUI_SlotCooldown : MonoBehaviour {
	
	private static Dictionary<int, float> spellCooldowns = new Dictionary<int, float>();
	
	public UISprite cooldownSprite;
	public UISprite finishSprite;
	public float finishOffset;
	public float fadingPct = 25f;
	public UILabel cooldownLabel;
	public GameObject cooldownSparklePrefab;

	public bool IsOnCooldown { get; private set; }
	
	private UISpellInfo currentSpellInfo;
	
	/// <summary>
	/// Raises the assign spell event.
	/// </summary>
	/// <param name="spellInfo">Spell info.</param>
	public void OnAssignSpell(UISpellInfo spellInfo)
	{
		// Save the spell info, very importatnt to be set before anything else
		this.currentSpellInfo = spellInfo;
		
		// Check if this spell still has cooldown
		if (spellCooldowns.ContainsKey(spellInfo.ID))
		{
			float cooldownTill = spellCooldowns[spellInfo.ID];
			
			// Check if the cooldown isnt expired
			if (cooldownTill > Time.time)
			{
				float remainingTime = cooldownTill - Time.time;
				
				// Start the remaing cooldown
				this.StartCooldown(remainingTime);
			}
			else
			{
				// Cooldown already expired, remove the record
				spellCooldowns.Remove(spellInfo.ID);
			}
		}
	}
	
	/// <summary>
	/// Raises the unassign event.
	/// </summary>
	public void OnUnassign()
	{
		this.InterruptCooldown();
		
		this.currentSpellInfo = null;
	}
	
	/// <summary>
	/// Starts a cooldown on the slot with the given duration.
	/// </summary>
	/// <param name="duration">Duration.</param>
	public void StartCooldown(float duration)
	{
		// Check if we have a cooldown sprite and we are assigned
		if (this.cooldownSprite == null || this.currentSpellInfo == null || this.currentSpellInfo.Cooldown <= 0f)
			return;
		
		// Enable the sprite if it's disabled
		if (!this.cooldownSprite.enabled)
			this.cooldownSprite.enabled = true;
		
		// Reset the fill amount
		this.cooldownSprite.fillAmount = 1f;
		
		// Enable the label if it's disabled
		if (this.cooldownLabel != null)
		{
			if (!this.cooldownLabel.enabled)
				this.cooldownLabel.enabled = true;
			
			this.cooldownLabel.text = duration.ToString("0");
		}

		if (this.finishSprite != null)
		{
			this.finishSprite.alpha = 0f;
			this.finishSprite.enabled = true;
			this.finishSprite.cachedTransform.localPosition = new Vector3(this.finishSprite.cachedTransform.localPosition.x, this.finishOffset, this.finishSprite.cachedTransform.localPosition.z);
		}

		// Set the slot on cooldown
		this.IsOnCooldown = true;
		
		// Save that this spell is on cooldown
		if (!spellCooldowns.ContainsKey(this.currentSpellInfo.ID))
			spellCooldowns.Add(this.currentSpellInfo.ID, (Time.time + duration));
		
		// Start the coroutine
		this.StartCoroutine("_StartCooldown", duration);
	}
	
	/// <summary>
	/// Interrupts the current cooldown.
	/// </summary>
	public void InterruptCooldown()
	{
		// Cancel the coroutine
		this.StopCoroutine("_StartCooldown");
		
		// Call the finish
		this.OnCooldownFinished(false);
	}
	
	IEnumerator _StartCooldown(float duration)
	{
		float cooldownDuration = this.currentSpellInfo.Cooldown;
		
		// Go back in time if we're resuming a cooldown
		float startTime = Time.time - (cooldownDuration - duration);
		
		while (Time.time < (startTime + cooldownDuration))
		{
			float RemainingTime = (startTime + cooldownDuration) - Time.time;
			float RemainingTimePct = RemainingTime / cooldownDuration;
			
			// Update the cooldown sprite
			if (this.cooldownSprite != null)
				this.cooldownSprite.fillAmount = RemainingTimePct;

			// Update the label
			if (this.cooldownLabel != null)
				this.cooldownLabel.text = RemainingTime.ToString("0");

			// Update the finish position
			if (this.finishSprite != null && this.cooldownSprite != null)
			{
				// Calculate the fill position
				float newY = (0f - ((float)this.cooldownSprite.height - (float)this.cooldownSprite.height * RemainingTimePct));

				// Add the offset
				newY += this.finishOffset;

				// Update the finish position
				this.finishSprite.cachedTransform.localPosition = new Vector3(this.finishSprite.cachedTransform.localPosition.x, newY, this.finishSprite.cachedTransform.localPosition.z);

				// Manage finish fading
				if (RemainingTimePct <= (this.fadingPct / 100))
				{
					this.finishSprite.alpha = (RemainingTimePct / (this.fadingPct / 100));
				}
				else if (RemainingTimePct >= (1f - (this.fadingPct / 100)))
				{
					this.finishSprite.alpha = (1f - (RemainingTimePct - (1f - (this.fadingPct / 100))) / (this.fadingPct / 100));
				}
				else if (RemainingTimePct > (this.fadingPct / 100) && RemainingTimePct < (1f - (this.fadingPct / 100)))
				{
					this.finishSprite.alpha = 1f;
				}
			}

			yield return 0;
		}
		
		// Call the on finish
		this.OnCooldownCompleted(true);
	}
	
	/// <summary>
	/// Raised when the cooldown completes it's full duration.
	/// </summary>
	/// <param name="sparkle">If set to <c>true</c> the sparkle effect will be executed.</param>
	private void OnCooldownCompleted(bool sparkle)
	{
		// Remove from the cooldowns list
		if (this.currentSpellInfo != null && spellCooldowns.ContainsKey(this.currentSpellInfo.ID))
			spellCooldowns.Remove(this.currentSpellInfo.ID);
		
		// Fire up the cooldown finished
		this.OnCooldownFinished(sparkle);
	}
	
	/// <summary>
	/// Raised when the cooldown finishes or has been interrupted.
	/// </summary>
	/// <param name="sparkle">If set to <c>true</c> the sparkle effect will be executed.</param>
	private void OnCooldownFinished(bool sparkle)
	{
		// No longer on cooldown
		this.IsOnCooldown = false;
		
		// Disable the sprite
		if (this.cooldownSprite != null)
			this.cooldownSprite.enabled = false;
		
		// Disable the label
		if (this.cooldownLabel != null)
			this.cooldownLabel.enabled = false;

		// Disable the finish sprite
		if (this.finishSprite != null)
			this.finishSprite.enabled = false;

		// Do a sparkle if defined
		if (sparkle && this.cooldownSparklePrefab != null)
		{
			GameObject spark = NGUITools.AddChild(this.transform.gameObject, this.cooldownSparklePrefab);

			// Apply it's prefab position
			spark.transform.localPosition = this.cooldownSparklePrefab.transform.localPosition;

			// Get the sprite and set it's depth
			UISprite sparkSprite = spark.GetComponent<UISprite>();
			
			if (sparkSprite != null)
				sparkSprite.depth = this.cooldownSprite.depth + 1;
		}
	}
}
