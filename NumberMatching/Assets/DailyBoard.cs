using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyBoard : MonoBehaviour{

    public Button m_oDailyButton;
    public Image m_oDailyButtonImage;
    private Color activeColor;
    private Color deactiveColor;



    private void Start() {
        activeColor = m_oDailyButtonImage.color;
        deactiveColor = new Color(activeColor.r, activeColor.g, activeColor.b, 0f);

        InternetConnectionCheck();
    }

    public void InternetConnectionCheck() {
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            HideIcon();
        }
        else {
            UnHideIcon();
        }
    }

    public void UnHideIcon() {
        m_oDailyButton.interactable = true;
        m_oDailyButtonImage.color = activeColor;
    }

    public void HideIcon() {

        m_oDailyButton.interactable = false;
        m_oDailyButtonImage.color = deactiveColor;

        StartCoroutine(CheckInternetAgain());
    }

    IEnumerator CheckInternetAgain() {
        yield return new WaitForSeconds(5f);
        InternetConnectionCheck();
    }

}
