using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIButton))]
[RequireComponent(typeof(SphereCollider))]
[ExecuteInEditMode]
public class Button : MonoBehaviour
{
    public enum ButtonType { None, StartGame, Login, Exit, Register, Next, Back, }
    public ButtonType buttonType = ButtonType.None;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Comm();
    }

    void Comm()
    {
        GetComponent<SphereCollider>().radius = GetComponent<UIWidget>().width / 2;
        GetComponent<UIButton>().tweenTarget = transform.FindChild("fill").gameObject;
        if (!GetComponent<UIButton>().onClick.Contains(new EventDelegate(this, "click")))
            GetComponent<UIButton>().onClick.Add(new EventDelegate(this, "click"));
        if(GetComponentInChildren<UILabel>())
            GetComponentInChildren<UILabel>().text = buttonType != ButtonType.None ? buttonType.ToString() : "";
    }


    public void click()
    {
        switch(buttonType)
        {
            case ButtonType.StartGame:
                GameControl.functionList.Add(delegate { return FUNCTIONS.ConnectionScene(); });
                break;
            case ButtonType.Login:
                GameControl.functionList.Add(delegate { return FUNCTIONS.SubmitLogin(FindObjectOfType<LoginUI>().Get); });
                break;
        }
    }
}
