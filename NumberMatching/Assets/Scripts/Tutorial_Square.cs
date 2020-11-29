using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Square : MonoBehaviour
{
    public int number = 0;
    public bool completed = false;

    public int gamePositionIndex;
    public int gamePositionX;
    public int gamePositionY;

    public List<Tutorial_Square> adjescentSquares = new List<Tutorial_Square>();
    public List<bool> adjescentConnections = new List<bool>() { false, false, false, false };
    public GameObject connection_group = default;
    [SerializeField] List<GameObject> faces = new List<GameObject>();
    private TutorialGameboard gameboard;
    public List<GameObject> solids = new List<GameObject>();

    public GameObject floatingGRP = default;
    private Vector3 floatingGRPScale;
    [SerializeField] SpriteRenderer squareSprite = default;

    public GameObject cube = default;
    [SerializeField] GameObject middleSquare = default;
    [SerializeField] List<Color> numberColors = new List<Color>();
    [SerializeField] List<Color> dimColors = new List<Color>();
    [SerializeField] List<Material> materialColors = new List<Material>();
    public List<Material> materialColors_fake = new List<Material>();

    public bool eyeMode = false;
    public bool eyePicker = false;
    public int eyePickerCountdown = 0;
    [SerializeField] Tutorial_EyePicking eyeModeMechanics = default;

    [SerializeField] GameObject sleepZzz = default;
    [SerializeField] List<Vector3> zposition = new List<Vector3>();
    private Coroutine zCoroutine = null;
    public float zDelayTimeMin;
    public float zDelayTimeMax;
    public float zzzTime;

    private Quaternion squareRotation;
    private Vector3 squarePosition;
    private Vector3 squareScale;

    public bool blocker = false;
    [SerializeField] Tutorial_RaycastForMouse ray = default;

    public float shaketime = 1f;
    public float shakemag = 1f;

    public AnimationCurve ease;

    [SerializeField] Tutorial_SwapButton switchButton = default;

    public List<GameObject> arrows = new List<GameObject>();

    private Coroutine dimCo;
    public float dimmingDuration = 0.5f;

    [Header("Swap Animation")]
    [SerializeField] private Transform route;
    private float tParam = 0f;
    private Vector2 routePos;
    public AnimationCurve swapEase;

    [SerializeField] GameObject purpleArrow = default;

    [SerializeField] SpriteRenderer sleepingDim = default;
    [SerializeField] Sprite dimMask = default;
    [SerializeField] Sprite dimMaskCutOut = default;
    public Color sleepColor;
    [SerializeField] List<GameObject> scoringEffect = new List<GameObject>();
    private int number4Effects = 0;


    private void Start()
    {
        floatingGRPScale = floatingGRP.transform.localScale;
        squareRotation = gameObject.transform.localRotation;
        squarePosition = gameObject.transform.localPosition;
        squareScale = gameObject.transform.localScale;
    }

    public void SetSquareDisplay()
    {
        NumberDisplay();
        EyeballDisplay();
    }

    private void NumberDisplay()
    {
        SetSquareColor();
        SetSquareFace();
        PopSFX();
        Pop();
    }

    private void SetSquareFace()
    {
        if (number < 5)
        {
            faces[number - 1].SetActive(true);
            faces[number - 1].GetComponent<FacialAnimation>().StartFacialAnimation();
        }
    }

    private void SetSquareColor()
    {
        squareSprite.color = numberColors[number - 1];
        cube.GetComponent<MeshRenderer>().material = materialColors[number - 1];
        LittleSquareDisplay();
    }

    private void LittleSquareDisplay()
    {
        if (number == 0 || number == 5)
        {
            middleSquare.SetActive(true);
        }
        else
        {
            middleSquare.SetActive(false);
        }
    }

    private void EyeballDisplay()
    {
        if (number < 5)
        {

            int eyeShutCounter = 0;
            for (int i = 0; i < adjescentConnections.Count; i++)
            {
                if (adjescentConnections[i] == true)
                {
                    eyeShutCounter++;
                }
            }

            faces[number - 1].GetComponent<FacialAnimation>().ShutThisManyEyes(eyeShutCounter);

        }
        gameboard.UpdateSquareConnections();
    }

    public void ConnectionDisplay()
    {
        SolidConnectionDisplay();
    }

    private void SolidConnectionDisplay()
    {
        connection_group.SetActive(true);
        for (int i = 0; i < adjescentConnections.Count; i++)
        {
            if (adjescentConnections[i] == true)
            {
                solids[i].SetActive(true);
            }
            else
            {
                solids[i].SetActive(false);
            }
        }
    }

    private void PopSFX()
    {
        switch (number)
        {
            case 1:
                SoundManager.SM.PlayOneShotSound("monster1");
                break;
            case 2:
                SoundManager.SM.PlayOneShotSound("monster2");
                break;
            case 3:
                SoundManager.SM.PlayOneShotSound("monster3");
                break;
            case 4:
                SoundManager.SM.PlayOneShotSound("monster4");
                break;
            default:
                //Debug.Log("nosfx");
                break;
        }
    }
    public void SquareDown()
    {
        gameObject.transform.localPosition = squarePosition;
    }

    private void Pop()
    {
        floatingGRP.transform.localScale = floatingGRPScale;
        gameObject.transform.localPosition = new Vector3(squarePosition.x, squarePosition.y, squarePosition.z - .3f);
        gameboard.AllSquaresDown(gameObject);

        squareSprite.sortingOrder = 1;
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 1f);
        hash.Add("oncomplete", "PopDone");
        hash.Add("oncompletetarget", gameObject);
        iTween.PunchScale(floatingGRP, hash);
    }

    private void PopDone()
    {
        floatingGRP.transform.localScale = floatingGRPScale;

        if (!eyeMode && switchButton.activated == false)
        {
            squareSprite.sortingOrder = 0;
        }
        SquareDown();
    }

    public void SetAdjescentSquares(TutorialGameboard gb)
    {
        gameboard = gb;
        adjescentSquares[0] = SetTopSquare();
        adjescentSquares[1] = SetBottomSquare();
        adjescentSquares[2] = SetLeftSquare();
        adjescentSquares[3] = SetRightSquare();
    }

    private Tutorial_Square SetTopSquare()
    {
        int topSquare_gamePositionY = gamePositionY + 1;
        int height = gameboard.gameBoardHeight;
        bool edgeCase = false;
        if (topSquare_gamePositionY >= height)
        {
            edgeCase = true;
        }
        if (!edgeCase)
        {
            int index = gamePositionIndex + 1;
            return gameboard.gameBoardSquares[index].GetComponent<Tutorial_Square>();
        }
        else
        {
            //Debug.Log(gameObject.name + " has no top");
            return null;
        }
    }

    private Tutorial_Square SetBottomSquare()
    {
        int botSquare_gamePositionY = gamePositionY - 1;
        bool edgeCase = false;
        if (botSquare_gamePositionY < 0)
        {
            edgeCase = true;
        }
        if (!edgeCase)
        {
            int index = gamePositionIndex - 1;
            return gameboard.gameBoardSquares[index].GetComponent<Tutorial_Square>();
        }
        else
        {
            //Debug.Log(gameObject.name + " has no bottom");
            return null;
        }
    }

    private Tutorial_Square SetLeftSquare()
    {
        int leftSquare_gamePositionX = gamePositionX - 1;
        bool edgeCase = false;
        if (leftSquare_gamePositionX < 0)
        {
            edgeCase = true;
        }
        if (!edgeCase)
        {
            int height = gameboard.gameBoardHeight;
            int index = gamePositionIndex - height;
            return gameboard.gameBoardSquares[index].GetComponent<Tutorial_Square>();
        }
        else
        {
            //Debug.Log(gameObject.name + " has no left");
            return null;
        }
    }

    private Tutorial_Square SetRightSquare()
    {
        int width = gameboard.gameBoardWidth;
        int rightSquare_gamePositionX = gamePositionX + 1;
        bool edgeCase = false;
        if (rightSquare_gamePositionX >= width)
        {
            edgeCase = true;
        }
        if (!edgeCase)
        {
            int height = gameboard.gameBoardHeight;
            int index = gamePositionIndex + height;
            return gameboard.gameBoardSquares[index].GetComponent<Tutorial_Square>();
        }
        else
        {
            //Debug.Log(gameObject.name + " has no right");
            return null;
        }
    }

    public void EyeModeUpdate()
    {
        Tutorial_Square eyePicker = GetEyePickerSquare();
        int adjescentIndex = GetAdjescentConnectionNumberOfEyePicker(eyePicker);
        adjescentConnections[adjescentIndex] = true;
        eyePicker.adjescentConnections[GetOppositeRandomOrderNumber(adjescentIndex)] = true;
        eyeMode = false;
        eyePicker.eyePickerCountdown--;
        eyeModeMechanics.UnHilightSquare(gameObject);
        CheckCompleted();
        eyePicker.CheckCompleted();

        if (eyePicker.eyePickerCountdown == 0)
        {
            eyePicker.WrapUpEyeMode();
        }
    }

    private void WrapUpEyeMode()
    {
        eyeModeMechanics.TurnOff();
    }

    private int GetCurrentTotalConnections()
    {
        int connections = 0;

        for (int i = 0; i < adjescentConnections.Count; i++)
        {
            if (adjescentConnections[i])
            {
                connections++;
            }
        }
        return connections;
    }

    public void CheckCompleted()
    {
        int connections = GetCurrentTotalConnections();

        if (connections == number)
        {
            completed = true;
            CheckDimIfCompleted();
            faces[number - 1].GetComponent<FacialAnimation>().StopFacialAnimation();
            EyeballDisplay();

            if(zCoroutine == null)
            {
                zCoroutine = StartCoroutine(DelayThenZzz());
            }
            else
            {
                StopCoroutine(zCoroutine);
                zCoroutine = StartCoroutine(DelayThenZzz());
            }


            gameboard.CheckForCompleteLink(gameObject);
            //Debug.Log(gameObject.name + "TurnOnZzzs");
        }
        else
        {
            completed = false;
            CheckDimIfCompleted();
            faces[number - 1].GetComponent<FacialAnimation>().StartFacialAnimation();
            EyeballDisplay();
        }
    }

    public void ResetSquare_OnFull()
    {
        PopAway();
        ZeroOutSquareInfo();
        StopZzz();
    }

    public void ResetSquare_OnCompletion_Before() {
        number4Effects = number;
        number = 0;
        adjescentConnections = new List<bool> { false, false, false, false };
        completed = false;
        blocker = false;
        SetSquarePosition();
    }

    public void ResetSquare_OnCompletion_After() {
        connection_group.SetActive(false);
        TurnOffFaces();
        squareSprite.GetComponent<CollectionColor_Sprite>().GetColor();
        cube.GetComponent<MeshRenderer>().material = materialColors[4];
        SetSquarePosition();
        LittleSquareDisplay();
        PopAway();
        PlaySpecialEffects();
        StopZzz();
    }

    public void ResetSquare_OnClick()
    {
        RemoveConnectionsOnAdjescentSquareInfo();
        ZeroOutSquareInfo();
    }

    private void RemoveConnectionsOnAdjescentSquareInfo()
    {

        for (int i = 0; i < adjescentConnections.Count; i++)
        {

            if (adjescentConnections[i] == true)
            {
                if (i == 0)
                {
                    adjescentSquares[i].adjescentConnections[1] = false;
                }
                else if (i == 1)
                {
                    adjescentSquares[i].adjescentConnections[0] = false;
                }
                else if (i == 2)
                {
                    adjescentSquares[i].adjescentConnections[3] = false;
                }
                if (i == 3)
                {
                    adjescentSquares[i].adjescentConnections[2] = false;
                }

                adjescentSquares[i].CheckCompleted();
            }

        }
    }

    public void ZeroOutSquareInfo()
    {
        connection_group.SetActive(false);
        number = 0;
        adjescentConnections = new List<bool> { false, false, false, false };
        completed = false;
        blocker = false;
        TurnOffFaces();
        squareSprite.GetComponent<CollectionColor_Sprite>().GetColor();
        cube.GetComponent<MeshRenderer>().material = materialColors[4];
        SetSquarePosition();
        LittleSquareDisplay();
    }

    private void SetSquarePosition()
    {
        floatingGRP.GetComponent<FloatingSquare>().StopFloat();
        gameObject.transform.localRotation = squareRotation;
        gameObject.transform.localPosition = squarePosition;
    }

    private void TurnOffFaces()
    {
        for (int i = 0; i < faces.Count; i++)
        {
            if (faces[i].activeSelf == true)
            {
                faces[i].GetComponent<FacialAnimation>().StopFacialAnimation();
                faces[i].GetComponent<FacialAnimation>().ResetEyes();
                faces[i].SetActive(false);
            }
        }
    }

    public void StopZzz()
    {
        if (zCoroutine != null)
        {
            StopCoroutine(zCoroutine);
        }
        //Debug.Log(gameObject.name + "TurnOffZzzs");
        //sleepZzz.GetComponent<ParticleSystem>().loop = true;
        sleepZzz.SetActive(false);
    }

    private void PopAway()
    {
        floatingGRP.transform.localScale = floatingGRPScale;
        squareSprite.sortingOrder = 1;
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 1f);
        hash.Add("oncomplete", "PopAwayDone");
        hash.Add("oncompletetarget", gameObject);
        iTween.PunchScale(floatingGRP, hash);
    }

    private void PlaySpecialEffects()
    {
        //Instantiate(specialEffects1, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        GameObject effect1 = (GameObject)Instantiate(scoringEffect[number4Effects - 1], gameObject.transform.position + new Vector3(0f, 0f, -1f), Quaternion.identity, gameObject.transform);
        effect1.transform.Rotate(new Vector3(210f, 0f, 0f));
    }

    private void PopAwayDone()
    {
        squareSprite.sortingOrder = 0;
        floatingGRP.transform.localScale = floatingGRPScale;
    }

    private IEnumerator DelayThenZzz()
    {
        sleepZzz.transform.localPosition = zposition[number - 1];
        float delayTime = UnityEngine.Random.Range(zDelayTimeMin, zDelayTimeMax);
        yield return new WaitForSeconds(delayTime);
        sleepZzz.SetActive(true);
        //float holdTime = UnityEngine.Random.Range(zHoldTimeMin, zHoldTimeMax);
        yield return new WaitForSeconds(zzzTime);
        //sleepZzz.GetComponent<ParticleSystem>().loop = false;
        //yield return new WaitForSeconds(2f);
        //sleepZzz.GetComponent<ParticleSystem>().loop = true;
        sleepZzz.SetActive(false);
        //Debug.Log("delayTime: " + delayTime);
        //Debug.Log("holdTime: " + holdTime);
    }
    private Tutorial_Square GetEyePickerSquare()
    {
        Tutorial_Square eyePickerSquare = null;

        for (int i = 0; i < gameboard.gameBoardSquares.Count; i++)
        {
            if (gameboard.gameBoardSquares[i].GetComponent<Tutorial_Square>().eyePicker)
            {
                eyePickerSquare = gameboard.gameBoardSquares[i].GetComponent<Tutorial_Square>();
            }
        }

        return eyePickerSquare;
    }

    private int GetAdjescentConnectionNumberOfEyePicker(Tutorial_Square matchingSquare)
    {
        int num = 0;
        for (int i = 0; i < adjescentSquares.Count; i++)
        {
            //Debug.LogWarning(adjescentSquares[i]);
            //Debug.LogWarning(matchingSquare);

            if (adjescentSquares[i] == matchingSquare)
            {
                //Debug.LogWarning("match "+ i);
                num = i;
            }
        }
        return num;
    }

    public void RecalculateAdjescentSquares()
    {
        for (int i = 0; i < adjescentSquares.Count; i++)
        {
            if (adjescentSquares[i] != null)
            {
                if (adjescentSquares[i].completed == false && adjescentSquares[i].number > 0)
                {
                    adjescentSquares[i].CalculateConnections();
                }
            }
        }
    }

    private int GetOppositeRandomOrderNumber(int number)
    {
        int oppositeNumber = 0;
        switch (number)
        {
            case 0:
                oppositeNumber = 1;
                break;
            case 1:
                oppositeNumber = 0;
                break;
            case 2:
                oppositeNumber = 3;
                break;
            case 3:
                oppositeNumber = 2;
                break;
        }
        return oppositeNumber;
    }

    private void ActivateAdjescentSquaresEyeMode()
    {
        eyePicker = true;

        for (int i = 0; i < adjescentSquares.Count; i++)
        {
            if (adjescentSquares[i] != null)
            {
                if (adjescentSquares[i].completed == false && adjescentSquares[i].number > 0)
                {
                    adjescentSquares[i].eyeMode = true;
                }
            }
        }

    }

    public void SwitchToFakeMaterials()
    {
        if(number-1 == -1)
        {
            cube.GetComponent<MeshRenderer>().material = materialColors_fake[4];
        }
        else
        {
            cube.GetComponent<MeshRenderer>().material = materialColors_fake[number - 1];
        }
    }

    private List<int> GetRandomOrder()
    {
        List<int> order = new List<int>() { 0, 1, 2, 3 };
        for (int i = 0; i < order.Count; i++)
        {
            int tempNumber = order[0];
            int randomIndex = UnityEngine.Random.Range(0, 4);
            order[0] = order[randomIndex];
            order[randomIndex] = tempNumber;
        }
        return order;
    }

    private int GetMovesAvailable(List<bool> allMoves)
    {
        int total = 0;
        for (int i = 0; i < allMoves.Count; i++)
        {
            if (allMoves[i] == true)
            {
                total++;
            }
        }
        return total;
    }

    public void CalculateConnections()
    {
        if (blocker == false)
        {
            List<bool> adjescentSquaresEligible = GetAdjescentSquaresEligible();
            List<int> randomOrder = GetRandomOrder();
            int currentConnections = GetCurrentTotalConnections();
            int movesAvailable = GetMovesAvailable(adjescentSquaresEligible);


            if (number < movesAvailable && ray.resetMode == false && switchButton.activated == false)
            {
                gameboard.eyePickingMode = true;
                eyePickerCountdown = number;
                ActivateAdjescentSquaresEyeMode();
                eyeModeMechanics.TurnOn();
            }

            else
            {
                for (int i = 0; i < adjescentSquaresEligible.Count; i++)
                {
                    if (currentConnections < number)
                    {
                        if (adjescentSquaresEligible[randomOrder[i]] == true)
                        {
                            adjescentConnections[randomOrder[i]] = true;
                            //Get Connection Squares info for this square.
                            int oppositeRandomOrderNumber = GetOppositeRandomOrderNumber(randomOrder[i]);
                            adjescentSquares[randomOrder[i]].adjescentConnections[oppositeRandomOrderNumber] = true;
                            //Debug.Log(gameObject.name + " CONNECTION " + adjescentSquares[randomOrder[i]].gameObject.name);
                            adjescentSquares[randomOrder[i]].CheckCompleted();
                            currentConnections++;
                        }
                    }
                }
                CheckCompleted();
            }
        }
    }

    private List<bool> GetAdjescentSquaresEligible()
    {
        List<bool> adjescentSquaresEligible = new List<bool> { false, false, false, false };
        adjescentSquaresEligible[0] = GetSquareEligibility(adjescentSquares[0], "Top");
        adjescentSquaresEligible[1] = GetSquareEligibility(adjescentSquares[1], "Bottom");
        adjescentSquaresEligible[2] = GetSquareEligibility(adjescentSquares[2], "Left");
        adjescentSquaresEligible[3] = GetSquareEligibility(adjescentSquares[3], "Right");
        return adjescentSquaresEligible;
    }

    private bool GetSquareEligibility(Tutorial_Square square, string side)
    {
        bool eligible = false;
        if (square != null)
        {
            if (square.completed == false && square.number > 0)
            {
                eligible = true;
                //Debug.Log(square.gameObject.name + " (" + side + ")" + " eligible");
            }
            //else {
            //Debug.Log(square.gameObject.name + " (" + side + ")" + " not eligible");
            //}
        }
        return eligible;
    }


    public void ShakeSquare()
    {
        StartCoroutine(Shake(shaketime, shakemag));
    }
    

    public IEnumerator Shake(float totalTime, float magnitudeMax)
    {

        Vector3 originalPosition = transform.localPosition;
        float elapsedTime = 0.0f;


        while (elapsedTime < totalTime)
        {

            float magnitude = Mathf.Lerp(0f, magnitudeMax, ease.Evaluate(elapsedTime / totalTime));
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        transform.localPosition = originalPosition;

    }


    public void CheckDimIfCompleted()
    {
        SetDimSprite();
        //Debug.LogWarning(squareSprite.transform.parent.parent.gameObject.name + " Check for Dimming!");
        if (completed)
        {

            if (dimCo == null)
            {
                dimCo = StartCoroutine(DimSquare());
            }
            else
            {
                StopCoroutine(dimCo);
                dimCo = StartCoroutine(DimSquare());
            }
        }

        else
        {
            //Debug.LogWarning(squareSprite.transform.parent.parent.gameObject.name + " " +completed);
            if (gameboard.playSquareClearAnimation == false)
            {
                if (dimCo == null)
                {
                    UnDimSquare();
                }
                else
                {
                    StopCoroutine(dimCo);
                    UnDimSquare();
                }
            }
        }

    }

    IEnumerator DimSquare()
    {
        if (number > 0 && number < 5)
        {
            if (sleepingDim.color != sleepColor)
            {
                Color alphaColor = new Color(sleepColor.r, sleepColor.g, sleepColor.b, 0f);
                for (float t = 0; t < dimmingDuration; t += Time.deltaTime)
                {
                    sleepingDim.color = Color.Lerp(alphaColor, sleepColor, t / dimmingDuration);
                    yield return null;
                }
                sleepingDim.color = sleepColor;
            }

        }
        else
        {
            int ColorIndex = GetColorIndex();
            if (ColorIndex > -1 && ColorIndex < 4)
            {
                if (sleepingDim.color != sleepColor)
                {
                    Color alphaColor = new Color(sleepColor.r, sleepColor.g, sleepColor.b, 0f);
                    for (float t = 0; t < dimmingDuration; t += Time.deltaTime)
                    {
                        sleepingDim.color = Color.Lerp(alphaColor, sleepColor, t / dimmingDuration);
                        yield return null;
                    }
                    sleepingDim.color = sleepColor;
                }

            }
        }
    }

    private void SetDimSprite()
    {
        if (adjescentConnections[1] == true)
        {
            sleepingDim.sprite = dimMaskCutOut;
        }
        else
        {
            sleepingDim.sprite = dimMask;
        }
    }

    public void UnDimSquare()
    {
        sleepingDim.color = new Color(sleepColor.r, sleepColor.g, sleepColor.b, 0f);
    }

    private int GetColorIndex()
    {
        int result = 4;
        if (squareSprite.color == numberColors[0])
        {
            result = 0;
        }
        else if (squareSprite.color == numberColors[1])
        {
            result = 1;
        }
        else if (squareSprite.color == numberColors[2])
        {
            result = 2;
        }
        else if (squareSprite.color == numberColors[3])
        {
            result = 3;
        }
        return result;
    }

    public void MoveAlongRoute(float timeDuration)
    {
        purpleArrow.SetActive(false);
        StartCoroutine(MoveSquareOnRoute(timeDuration));
        StartCoroutine(ScaleSquareOnRoute(timeDuration));
    }

    IEnumerator MoveSquareOnRoute(float timeDuration)
    {
        Vector2 p0 = route.GetChild(0).position;
        Vector2 p1 = route.GetChild(1).position;
        Vector2 p2 = route.GetChild(2).position;
        Vector2 p3 = route.GetChild(3).position;


        for (float t = 0f; t < timeDuration; t += Time.deltaTime)
        {
            tParam = swapEase.Evaluate(t / timeDuration);

            routePos = Mathf.Pow(1 - tParam, 3) * p0 +
            3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
            3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
            Mathf.Pow(tParam, 3) * p3;

            gameObject.transform.position = routePos;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;
        gameObject.transform.localPosition = squarePosition;
        
    }

    IEnumerator ScaleSquareOnRoute(float timeDuration)
    {
        Vector3 newScale = new Vector3(1.03f, 1.03f, 1f);

        for (float t = 0f; t < timeDuration; t += Time.deltaTime)
        {
            gameObject.transform.localScale = Vector3.Lerp(squareScale, newScale, swapEase.Evaluate(t / timeDuration));
            yield return new WaitForEndOfFrame();
        }

        gameObject.transform.localScale = squareScale;
    }

    public void HideConnections()
    {
        for (int i = 0; i < adjescentConnections.Count; i++)
        {
            if (adjescentConnections[i] == true)
            {
                solids[i].SetActive(false);


                int opposite_i = GetOppositeRandomOrderNumber(i);
                adjescentSquares[i].solids[opposite_i].SetActive(false);
            }
        }
    }

    public void WakeUpNeighbors()
    {
        for (int i = 0; i < adjescentConnections.Count; i++)
        {
            if (adjescentConnections[i] == true)
            {
                int eyeShutCounter = 0;

                for (int j = 0; j < adjescentSquares[i].adjescentConnections.Count; j++)
                {
                    if (adjescentSquares[i].adjescentConnections[j] == true)
                    {
                        eyeShutCounter++;
                    }

                }
                //Debug.Log(eyeShutCounter);
                adjescentSquares[i].faces[adjescentSquares[i].number - 1].GetComponent<FacialAnimation>().ShutThisManyEyes(eyeShutCounter - 1);
            }
        }
    }
}
