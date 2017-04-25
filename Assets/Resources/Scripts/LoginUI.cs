using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LoginUI : MonoBehaviour
{
    UIButton btn;
    UIInput user, pass;

	void Update ()
    {
        btn = GetComponentInChildren<UIButton>();
        if (!btn) return;
        user = transform.FindChild("username").GetComponentInChildren<UIInput>();
        if (!user) return;
        pass = transform.FindChild("password").GetComponentInChildren<UIInput>();
        if (!pass) return;

        if (!btn.onClick.Contains(new EventDelegate(this, "click")))
            btn.onClick.Add(new EventDelegate(this, "click"));
    }

    public void click()
    {
        GameControl.functionList.Add(delegate { return FUNCTIONS.SubmitLogin(user, pass);  });
    }
}
