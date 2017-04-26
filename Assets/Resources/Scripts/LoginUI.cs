using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LoginUI : MonoBehaviour
{
    UIButton btn;
    UIInput user, pass;

    public UIInput[] Get { get { return new UIInput[] { user, pass }; } }

    void Update ()
    {
        btn = GetComponentInChildren<UIButton>();
        if (!btn) return;
        user = transform.FindChild("username").GetComponentInChildren<UIInput>();
        if (!user) return;
        pass = transform.FindChild("password").GetComponentInChildren<UIInput>();
        if (!pass) return;
    }
}
