using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialNextBoard : MonoBehaviour
{
    [SerializeField] Image mainSquare = default;
    [SerializeField] Image secondSquare = default;
    [SerializeField] Image thirdSquare = default;

    public List<Color> colors;
    public int mainNumber;
    public int secondNumber;
    public int thirdNumber;
    [SerializeField] List<GameObject> nextSquares = new List<GameObject>();
    private List<Vector3> pos = new List<Vector3>();
    [SerializeField] List<GameObject> faces = new List<GameObject>();
    public List<Coroutine> cors = new List<Coroutine>();
    public float moveDuration = .5f;

    private void Start()
    {
        GetPositions();
        GetRandomNumbers();
        ColorDisplay();
    }

    private void GetPositions()
    {
        for (int i = 0; i < nextSquares.Count; i++)
        {
            pos.Add(nextSquares[i].transform.localPosition);
        }
    }

    public void ColorDisplay()
    {
        Color mainColor = new Color(colors[mainNumber].r, colors[mainNumber].g, colors[mainNumber].b, 1f);
        mainSquare.color = mainColor;

        secondSquare.color = colors[secondNumber];
        thirdSquare.color = colors[thirdNumber];
        SetFace();
    }

    public void FakeColorDisplay()
    {
        secondSquare.color = colors[mainNumber];
        thirdSquare.color = colors[secondNumber];
    }

    private void SetFace()
    {
        for (int i = 0; i < faces.Count; i++)
        {
            faces[i].SetActive(false);
        }
        faces[mainNumber].SetActive(true);
    }

    private void GetRandomNumbers()
    {
        mainNumber = UnityEngine.Random.Range(0, 2);
        secondNumber = UnityEngine.Random.Range(0, 2);
        thirdNumber = UnityEngine.Random.Range(0, 2);
    }

    public void RotateNextBoard()
    {
        mainNumber = secondNumber;
        secondNumber = thirdNumber;
        thirdNumber = UnityEngine.Random.Range(0, 2);
        //ColorDisplay();
        NextboardSquareAnimationsAndDisplays();
    }


    private void NextboardSquareAnimationsAndDisplays()
    {
        FixPositions();

        nextSquares[0].gameObject.SetActive(false);
        for (int i = 1; i < nextSquares.Count; i++)
        {
            Coroutine c = StartCoroutine(MoveOverTime(nextSquares[i].gameObject, pos[i], pos[i - 1]));
            cors.Add(c);
        }
        Coroutine c2 = StartCoroutine(TurnOnFirstSquare());
        cors.Add(c2);
    }

    private void FixPositions()
    {
        for (int i = 0; i < cors.Count; i++)
        {
            Debug.LogWarning("stopping corotines");
            StopCoroutine(cors[i]);

            if (i != 0)
            {
                //nextSquares[i].SetFakeDisplay(nextSquares[i - 1].number);
                FakeColorDisplay();
            }

        }
        cors.Clear();

        for (int i = 0; i < nextSquares.Count; i++)
        {
            nextSquares[i].transform.localPosition = pos[i];
        }
    }

    IEnumerator TurnOnFirstSquare()
    {
        yield return new WaitForSeconds(moveDuration);
        nextSquares[0].gameObject.SetActive(true);

        PopFirstSquare();

        ColorDisplay();
        cors.Clear();
    }

    public void PopFirstSquare()
    {
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(nextSquares[0], hash);
    }

    IEnumerator MoveOverTime(GameObject square, Vector3 oldPos, Vector3 newPos)
    {
        for (float t = 0; t < moveDuration; t += Time.deltaTime)
        {
            square.transform.localPosition = Vector3.Lerp(oldPos, newPos, t / moveDuration);
            yield return null;
        }

        square.transform.localPosition = oldPos;
        //square.GetComponent<SquareMechanics_Next>().SetNumberDisplay();
        ColorDisplay();
    }

}
