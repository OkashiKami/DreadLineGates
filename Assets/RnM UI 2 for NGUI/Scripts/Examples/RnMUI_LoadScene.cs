using UnityEngine;
using System.Collections;

public class RnMUI_LoadScene : MonoBehaviour {

	public string sceneName;
	public UIProgressBar bar;
	public float requiredValue = 1f;
	public bool ignoreFirst = true;
	
	void Start()
	{
		if (this.bar != null)
			this.bar.onChange.Add(new EventDelegate(OnProgress));
	}
	
	public void LoadScene()
	{
		Application.LoadLevel(sceneName);
	}

	public void OnProgress()
	{
		if (this.ignoreFirst)
		{
			this.ignoreFirst = false;
			return;
		}

		if (this.bar != null && this.bar.value >= this.requiredValue)
			LoadScene();
	}
}
