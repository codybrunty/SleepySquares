using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Instructions_2 : MonoBehaviour{

    [SerializeField] GameObject green = default;
    [SerializeField] GameObject green1 = default;
    [SerializeField] GameObject green2 = default;
    [SerializeField] GameObject red = default;
    [SerializeField] GameObject purple = default;

    [SerializeField] SpriteRenderer green_sr = default;
    [SerializeField] SpriteRenderer green1_sr = default;
    [SerializeField] SpriteRenderer green2_sr = default;
    [SerializeField] SpriteRenderer red_sr = default;
    [SerializeField] SpriteRenderer purple_sr = default;

    [SerializeField] GameObject connection1 = default;
    [SerializeField] GameObject connection2 = default;
    [SerializeField] GameObject connection3 = default;
    [SerializeField] GameObject connection4 = default;
    [SerializeField] FacialAnimation green_face = default;
    [SerializeField] FacialAnimation green1_face = default;
    [SerializeField] FacialAnimation green2_face = default;
    [SerializeField] FacialAnimation red_face = default;
    [SerializeField] FacialAnimation purple_face = default;
    [SerializeField] TutorialGoNext goNext = default;
    [SerializeField] GameObject scoreboard = default;
    [SerializeField] GameObject zzz = default;
    //[SerializeField] List<Color> dimColors = default;
    public float dimmingDuration = 0.25f;
    public float scalingDuration = 0.35f;

    private void OnEnable() {
        StartCoroutine(Tutorial2_Animations());
    }

    IEnumerator Tutorial2_Animations() {
        yield return new WaitForSeconds(1f);
        MoveGreen();
        yield return new WaitForSeconds(1f);
        ConnectionAppear(connection1);
        Pop(green);
        Pop(connection1);
        Pop(red);
        yield return new WaitForSeconds(1f);
        green_face.ShutThisManyEyes(1);
        DimSquare(green);
        red_face.ShutThisManyEyes(1);
        yield return new WaitForSeconds(1f);
        MovePurple();
        zzz.SetActive(true);
        yield return new WaitForSeconds(1f);
        ConnectionAppear(connection2);
        Pop(red);
        Pop(connection2);
        Pop(purple);
        yield return new WaitForSeconds(1f);

        //zzz.GetComponent<ParticleSystem>().loop = false;
        purple_face.ShutThisManyEyes(1);
        red_face.ShutThisManyEyes(2);
        DimSquare(red);

        yield return new WaitForSeconds(1f);

        Green1Pop();
        Green2Pop();
        yield return new WaitForSeconds(1f);
        MoveGreen1();
        MoveGreen2();

        yield return new WaitForSeconds(1f);
        Pop(green1);
        Pop(green2);
        Pop(purple);
        Pop(connection3);
        Pop(connection4);
        yield return new WaitForSeconds(1f);
        green1_face.ShutThisManyEyes(1);
        green2_face.ShutThisManyEyes(1);
        purple_face.ShutThisManyEyes(3);
        DimSquare(purple);
        DimSquare(green1);
        DimSquare(green2);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MoveScoreDown());
        yield return new WaitForSeconds(0.1f);

        zzz.SetActive(false);
        goNext.animationDone = true;
    }


    public void DimSquare(GameObject squareGO)
    {
        squareGO.GetComponent<Tutorial_Square>().completed = true;
        squareGO.GetComponent<Tutorial_Square>().CheckDimIfCompleted();
    }

    IEnumerator MoveScoreDown()
    {
        Vector3 newPos = scoreboard.transform.localPosition;
        Vector3 oldPos = new Vector3(newPos.x,newPos.y+500f,newPos.z);
        scoreboard.transform.localPosition = oldPos;
        scoreboard.SetActive(true);
        for (float t = 0; t < 0.5f; t+=Time.deltaTime)
        {
            scoreboard.transform.localPosition = Vector3.Lerp(oldPos,newPos,t/0.5f);
            yield return null;
        }
        scoreboard.transform.localPosition = newPos;
    }

    private void MoveGreen()
    {
        Vector3 newPosition = new Vector3(green.transform.position.x + 0.25f, green.transform.position.y, green.transform.position.z);
        Hashtable hash = new Hashtable();
        hash.Add("position", newPosition);
        hash.Add("time", 1f);
        iTween.MoveTo(green, hash);
    }
    private void MoveGreen1()
    {
        Vector3 newPosition = new Vector3(green1.transform.position.x, green1.transform.position.y - 0.25f, green1.transform.position.z);
        Hashtable hash = new Hashtable();
        hash.Add("position", newPosition);
        hash.Add("time", 1f);
        iTween.MoveTo(green1, hash);
    }

    private void MoveGreen2()
    {
        Vector3 newPosition = new Vector3(green2.transform.position.x, green2.transform.position.y + 0.25f, green2.transform.position.z);
        Hashtable hash = new Hashtable();
        hash.Add("position", newPosition);
        hash.Add("time", 1f);
        iTween.MoveTo(green2, hash);
    }
    private void MovePurple()
    {
        Vector3 newPosition = new Vector3(purple.transform.position.x - 0.25f, purple.transform.position.y, purple.transform.position.z);
        Hashtable hash = new Hashtable();
        hash.Add("position", newPosition);
        hash.Add("time", 1f);
        iTween.MoveTo(purple, hash);
    }

    private void Green1Pop()
    {
        green1.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(green1, hash);
        FindObjectOfType<SoundManager>().PlayOneShotSound("monster1");
    }

    private void Green2Pop()
    {
        green2.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(green2, hash);
    }

    private void Pop(GameObject go)
    {
        go.SetActive(true);
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(go, hash);
        FindObjectOfType<SoundManager>().PlayOneShotSound("connect");
    }

    private void ConnectionAppear(GameObject connection)
    {
        connection.SetActive(true);
    }


}
