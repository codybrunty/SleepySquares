using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPocketMazes : MonoBehaviour
{

    public void PocketMazesOnClick()
    {
        SoundManager.SM.PlayOneShotSound("select1");

#if UNITY_ANDROID
                Application.OpenURL("market://details?id=com.BombChomp.PocketMazes");
#elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/id1503400446");
        #endif
    }
}
