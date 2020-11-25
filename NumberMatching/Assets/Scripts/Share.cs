using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Share : MonoBehaviour{

    [SerializeField] GameOverPanel gameOverPanel = default;
    private string shareMessage;
    private long score = 0;

    public void ShareScore() {
        GetScore();
        shareMessage = "Wooo, I just scored " + score + " points! I challenge you to beat me in #SleepySquares";
        //Debug.Log(shareMessage);
        StartCoroutine(TakeScreenShotAndShare());
    }

    private void GetScore() {
        score = gameOverPanel.score;
    }

    private IEnumerator TakeScreenShotAndShare() {

        yield return new WaitForEndOfFrame();

        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared_img.png");
        File.WriteAllBytes(filePath, screenShot.EncodeToPNG());

        Destroy(screenShot);

        new NativeShare().AddFile(filePath).SetSubject("Sleepy Squares").SetText(shareMessage).Share();

    }

}
