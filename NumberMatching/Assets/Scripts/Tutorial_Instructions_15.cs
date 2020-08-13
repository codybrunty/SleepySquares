using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Instructions_15 : MonoBehaviour{

    [SerializeField] GameObject smallBoard = default;
    [SerializeField] GameObject bigBoard = default;
    [SerializeField] GameObject clickNext = default;
    [SerializeField] GameObject instruct1 = default;
    [SerializeField] GameObject instruct2 = default;
    [SerializeField] GameObject instruct3 = default;
    [SerializeField] Color greenColor = default;
    [SerializeField] Color redColor = default;
    [SerializeField] Color purpleColor = default;

    [SerializeField] GameObject red1_GO = default;
    [SerializeField] GameObject red1_face = default;
    [SerializeField] SpriteRenderer red1_square = default;

    [SerializeField] GameObject purple1_GO = default;
    [SerializeField] GameObject purple1_face = default;
    [SerializeField] SpriteRenderer purple1_square = default;

    [SerializeField] GameObject green1_GO = default;
    [SerializeField] GameObject green1_face = default;
    [SerializeField] SpriteRenderer green1_square = default;

    [SerializeField] GameObject green2_GO = default;
    [SerializeField] GameObject green2_face = default;
    [SerializeField] SpriteRenderer green2_square = default;

    [SerializeField] GameObject red2_GO = default;
    [SerializeField] GameObject red2_face = default;
    [SerializeField] SpriteRenderer red2_square = default;

    [SerializeField] GameObject green3_GO = default;
    [SerializeField] GameObject green3_face = default;
    [SerializeField] SpriteRenderer green3_square = default;

    [SerializeField] GameObject next1 = default;
    [SerializeField] GameObject next2 = default;
    [SerializeField] GameObject next3 = default;

    [SerializeField] List<GameObject> move1_eyes = new List<GameObject>();
    [SerializeField] List<GameObject> move1_eyelids = new List<GameObject>();
    [SerializeField] List<GameObject> move2_eyes = new List<GameObject>();
    [SerializeField] List<GameObject> move2_eyelids = new List<GameObject>();
    [SerializeField] List<GameObject> move3_eyes = new List<GameObject>();
    [SerializeField] List<GameObject> move3_eyelids = new List<GameObject>();
    [SerializeField] List<GameObject> move4_eyes = new List<GameObject>();
    [SerializeField] List<GameObject> move4_eyelids = new List<GameObject>();

    [SerializeField] Color brokenColor = default;
    [SerializeField] GameObject smoke1 = default;
    [SerializeField] GameObject smoke2 = default;
    [SerializeField] GameObject smoke3 = default;
    [SerializeField] GameObject smoke4 = default;
    [SerializeField] GameObject smoke5 = default;
    [SerializeField] GameObject smoke6 = default;
    [SerializeField] GameObject smoke7 = default;
    [SerializeField] GameObject smoke8 = default;
    [SerializeField] GameObject smoke9 = default;
    [SerializeField] SpriteRenderer brokenSquare1 = default;
    [SerializeField] SpriteRenderer brokenSquare2 = default;
    [SerializeField] SpriteRenderer brokenSquare3 = default;
    [SerializeField] SpriteRenderer brokenSquare4 = default;
    [SerializeField] SpriteRenderer brokenSquare5 = default;
    [SerializeField] SpriteRenderer brokenSquare6 = default;
    [SerializeField] SpriteRenderer brokenSquare7 = default;
    [SerializeField] SpriteRenderer brokenSquare8 = default;
    [SerializeField] SpriteRenderer brokenSquare9 = default;
    [SerializeField] GameObject flashyButton = default;


    private void OnEnable() {
        clickNext.SetActive(false);
        StartCoroutine(Tutorial14_Animations());
    }

    IEnumerator Tutorial14_Animations() {
        MoveSmallBoard();
        yield return new WaitForSeconds(1.5f);
        ScaleBigBoard();
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(FillBoard());
        yield return new WaitForSeconds(2.5f);
        ChangeInstructions();
        yield return new WaitForSeconds(7.5f);
        ChangeInstructions2();
        yield return new WaitForSeconds(2.5f);
        MoveAndScaleClickNext();
        clickNext.SetActive(true);
        yield return new WaitForSeconds(1f);
        flashyButton.SetActive(true);
    }

    IEnumerator FillBoard() {
        Purple1Punch();
        UpdateNextBoard(0,0,1);
        yield return new WaitForSeconds(.5f);
        Green2Punch();
        UpdateNextBoard(0, 1, 1);
        yield return new WaitForSeconds(.5f);
        Move1_CloseEyes();
        yield return new WaitForSeconds(.25f);
        Green3Punch();
        UpdateNextBoard(1, 1, 0);
        yield return new WaitForSeconds(.5f);
        Red1Punch();
        UpdateNextBoard(1, 0, 0);
        yield return new WaitForSeconds(.5f);
        Move2_CloseEyes();
        yield return new WaitForSeconds(.25f);
        Red2Punch();
        UpdateNextBoard(0, 0, 2);
        yield return new WaitForSeconds(.5f);
        Move3_CloseEyes();
        yield return new WaitForSeconds(.25f);
        Green1Punch();
        UpdateNextBoard(0, 2, 0);
        yield return new WaitForSeconds(.5f);
        Move4_CloseEyes();
        yield return new WaitForSeconds(1f);
        BreakSquare(brokenSquare1, smoke1);
        yield return new WaitForSeconds(.5f);
        BreakSquare(brokenSquare2, smoke2);

        yield return new WaitForSeconds(.5f);
        BreakSquare(brokenSquare3, smoke3);
        yield return new WaitForSeconds(.5f);
        BreakSquare(brokenSquare4, smoke4);
        yield return new WaitForSeconds(.5f);
        BreakSquare(brokenSquare5, smoke5);
        yield return new WaitForSeconds(.5f);
        BreakSquare(brokenSquare6, smoke6);
        yield return new WaitForSeconds(.5f);
        BreakSquare(brokenSquare7, smoke7);
        yield return new WaitForSeconds(.5f);
        BreakSquare(brokenSquare8, smoke8);
        yield return new WaitForSeconds(.5f);
        BreakSquare(brokenSquare9, smoke9);

    }

    private void BreakSquare(SpriteRenderer square, GameObject smoke) {
        smoke.SetActive(true);
        square.color = brokenColor;
    }


    private void Move4_CloseEyes() {
        foreach (GameObject eye in move4_eyes) {
            eye.SetActive(false);
        }
        foreach (GameObject eyelid in move4_eyelids) {
            eyelid.SetActive(true);
        }
    }

    private void Move3_CloseEyes() {
        foreach (GameObject eye in move3_eyes) {
            eye.SetActive(false);
        }
        foreach (GameObject eyelid in move3_eyelids) {
            eyelid.SetActive(true);
        }
    }

    private void Move2_CloseEyes() {
        foreach (GameObject eye in move2_eyes) {
            eye.SetActive(false);
        }
        foreach (GameObject eyelid in move2_eyelids) {
            eyelid.SetActive(true);
        }
    }

    private void Move1_CloseEyes() {
        foreach (GameObject eye in move1_eyes) {
            eye.SetActive(false);
        }
        foreach (GameObject eyelid in move1_eyelids) {
            eyelid.SetActive(true);
        }
    }

    private void UpdateNextBoard(int first, int second, int third) {
        List<Color> allColors = new List<Color> { greenColor, redColor, purpleColor };
        next1.GetComponent<Image>().color = allColors[first];
        next2.GetComponent<Image>().color = allColors[second];
        next3.GetComponent<Image>().color = allColors[third];
        PunchNextSquare();
    }

    private void PunchNextSquare() {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(next1, hash);
    }

    private void Green1Punch() {
        green1_square.color = greenColor;
        green1_face.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(green1_GO, hash);
        FindObjectOfType<SoundManager>().PlayOneShotSound("monster1");
    }

    private void Green2Punch() {
        green2_square.color = greenColor;
        green2_face.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(green2_GO, hash);
        FindObjectOfType<SoundManager>().PlayOneShotSound("monster1");
    }

    private void Green3Punch() {
        green3_square.color = greenColor;
        green3_face.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(green3_GO, hash);
        FindObjectOfType<SoundManager>().PlayOneShotSound("monster1");
    }

    private void Red2Punch() {
        red2_square.color = redColor;
        red2_face.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(red2_GO, hash);
        FindObjectOfType<SoundManager>().PlayOneShotSound("monster2");
    }


    private void Red1Punch() {
        red1_square.color = redColor;
        red1_face.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(red1_GO, hash);
        FindObjectOfType<SoundManager>().PlayOneShotSound("monster2");
    }

    private void Purple1Punch() {
        purple1_square.color = purpleColor;
        purple1_face.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(purple1_GO, hash);
        FindObjectOfType<SoundManager>().PlayOneShotSound("monster3");
    }

    private void ChangeInstructions() {
        instruct1.SetActive(false);
        instruct2.SetActive(true);
    }
    private void ChangeInstructions2() {
        instruct2.SetActive(false);
        instruct3.SetActive(true);
    }

    private void ScaleBigBoard() {

        Hashtable hash = new Hashtable();
        hash.Add("scale", new Vector3(1f, 1f, 1f));
        hash.Add("time", 1f);
        

        bigBoard.SetActive(true);
        foreach (Transform child in bigBoard.transform) {
            iTween.ScaleTo(child.gameObject, hash);
        }
    }

    private void MoveSmallBoard() {
        Vector3 hidePosition = new Vector3(smallBoard.transform.position.x+10f, smallBoard.transform.position.y, smallBoard.transform.position.z);
        Hashtable hash = new Hashtable();
        hash.Add("position", hidePosition);
        hash.Add("time", 1f);
        hash.Add("oncomplete", "HideSmallBoard");
        hash.Add("oncompletetarget", gameObject);
        iTween.MoveTo(smallBoard, hash);
    }

    private void HideSmallBoard() {
        smallBoard.SetActive(false);
    }

    private void MoveAndScaleClickNext() {
        clickNext.transform.localPosition = new Vector3(1.806f, 2.536f, 5f);
        clickNext.transform.localScale = new Vector3(3.35322f, 7.073081f, 3.987f);
    }

}
