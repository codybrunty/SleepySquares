using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPrivacyPolicy : MonoBehaviour
{

    public void PrivacyPolicyOnClick()
    {
        Application.OpenURL("https://www.bombchomp.com/terms");
    }

}
