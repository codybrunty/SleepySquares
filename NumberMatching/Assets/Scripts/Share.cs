using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Share : MonoBehaviour{


    /*
    private int highScore = 55;
    private string twitterAddress = "https://www.twitter.com/intent/tweet";
    private string twitterLanguage = "en";
    private string twitterTweetBeg = "SleepyHeadz New HighScore: ";
    private string twitterTweetEnd = " Good Luck Beating that! #BombChomp";
    private string twitterLinkAttached = "https://www.google.com";

    public void ShareScoreOnTwitter() {
        Application.OpenURL(twitterAddress+"?text="+WWW.EscapeURL(twitterTweetBeg) + highScore + WWW.EscapeURL(twitterTweetEnd) + WWW.EscapeURL(twitterLinkAttached) + "&amp;lang="+twitterLanguage+WWW.EscapeURL(twitterLanguage));
    }

    */
    private string shareMessage;

    public void ShareScore() {
        shareMessage = "Wooo, I can't believe I just scored 55 points in SleepyHeadz";
        StartCoroutine(TakeScreenShotAndShare());
    }

    private IEnumerator TakeScreenShotAndShare() {

        yield return new WaitForEndOfFrame();

        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, screenShot.EncodeToPNG());

        Destroy(screenShot);

        new NativeShare().AddFile(filePath).SetSubject("SleepyHeadz").SetText(shareMessage).Share();

    }

}
