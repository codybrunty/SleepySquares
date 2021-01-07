using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialScoreboard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    public int score = 8;
    [SerializeField] TextMeshProUGUI floatingText = default;

    private void Start()
    {
        text.text = score.ToString();
    }

    public void ScoreBoardDisplay()
    {
        text.text = score.ToString();
    }

    public void ScoreboardAdd(int number)
    {
        score += number;
        ScoreBoardDisplay();
        StartCoroutine(PopAnim(number));
    }

    IEnumerator PopAnim(int number)
    {

        yield return new WaitForSeconds(0.1f);

        floatingText.text = "+" + number;
        floatingText.gameObject.GetComponent<FloatingText>().FlashText();

        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(2, 2f, 0f));
        hash.Add("time", 1.5f);
        iTween.PunchScale(gameObject, hash);
    }
}
