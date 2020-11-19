using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Instructions_4 : MonoBehaviour{

    [SerializeField] List<GameObject> invisiblieWhites = default;
    [SerializeField] GameObject raycastGO = default;
    [SerializeField] GameObject nextBoard = default;
    [SerializeField] GameObject nextBoard_bg = default;
    [SerializeField] GameObject gameboard_bg = default;

    [SerializeField] List<MoveToNextSquarePosition> point4s = new List<MoveToNextSquarePosition>();

    private void OnEnable() {
        StartCoroutine(Tutorial4_Animations());
    }

    IEnumerator Tutorial4_Animations()
    {
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(MoveNextBoard());
        StartCoroutine(MoveNextBoardBG());
        PopWhiteBoard();
        PopBG();
        yield return new WaitForSeconds(0.5f);
        raycastGO.SetActive(true);

    }

    IEnumerator MoveNextBoard()
    {

        Vector3 newPos = nextBoard.transform.localPosition;
        Vector3 oldPos = new Vector3(newPos.x, newPos.y + 500f, newPos.z);
        nextBoard.transform.localPosition = oldPos;
        nextBoard.SetActive(true);

        for (float t = 0; t < 0.5f; t += Time.deltaTime)
        {
            nextBoard.transform.localPosition = Vector3.Lerp(oldPos, newPos, t / 0.5f);
            yield return null;
        }
        nextBoard.transform.localPosition = newPos;
        ReAdjust4Points();
    }

    IEnumerator MoveNextBoardBG()
    {

        Vector3 newPos = nextBoard_bg.transform.localPosition;
        Vector3 oldPos = new Vector3(newPos.x, newPos.y + 500f, newPos.z);
        nextBoard_bg.transform.localPosition = oldPos;
        nextBoard_bg.SetActive(true);

        for (float t = 0; t < 0.5f; t += Time.deltaTime)
        {
            nextBoard_bg.transform.localPosition = Vector3.Lerp(oldPos, newPos, t / 0.5f);
            yield return null;
        }
        nextBoard_bg.transform.localPosition = newPos;
    }

    private void ReAdjust4Points()
    {
        for (int i = 0; i < point4s.Count; i++)
        {
            point4s[i].AdjustPosition();
        }
    }

    private void PopBG()
    {
        gameboard_bg.SetActive(true);
    }

    private void PopWhiteBoard()
    {
        for (int i = 0; i < invisiblieWhites.Count; i++)
        {
            Pop(invisiblieWhites[i]);
        }
        FindObjectOfType<SoundManager>().PlayOneShotSound("clearBlockers");
    }

    private void Pop(GameObject go)
    {
        go.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(go, hash);
    }
}
