using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Share : MonoBehaviour{

    private int highScore = 55;
    private string twitterAddress = "https://www.twitter.com/intent/tweet";
    private string twitterLanguage = "en";
    private string twitterTweetBeg = "SleepyHeadz New HighScore: ";
    private string twitterTweetEnd = " Good Luck Beating that! #BombChomp";
    private string twitterLinkAttached = "https://www.google.com";

    public void ShareScoreOnTwitter() {
        Application.OpenURL(twitterAddress+"?text="+WWW.EscapeURL(twitterTweetBeg) + highScore + WWW.EscapeURL(twitterTweetEnd) + WWW.EscapeURL(twitterLinkAttached) + "&amp;lang="+twitterLanguage+WWW.EscapeURL(twitterLanguage));
    }
}
