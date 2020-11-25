using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SquareMechanics_Gameboard : MonoBehaviour {

    [Header("Square Info")]
    public int number = 0;
    public bool completed = false;
    public bool blocker = false;
    public List<bool> adjescentConnections = new List<bool>() { false, false, false, false };
    [Header("GameBoard Info")]
    public int gamePositionX = 0;
    public int gamePositionY = 0;
    public int gamePositionIndex = 0;
    public List<SquareMechanics_Gameboard> adjescentSquares = new List<SquareMechanics_Gameboard>() { null, null, null, null };
    public List<GameObject> solids = new List<GameObject>();
    public GameObject connection_group = default;
    [Header("Game Objects")]
    [SerializeField] SpriteRenderer squareSprite = default;
    [SerializeField] List<FacialAnimation> faces = new List<FacialAnimation>();
    [SerializeField] List<Color> numberColors = new List<Color>();
    [SerializeField] List<Material> materialColors = new List<Material>();
    [SerializeField] List<Material> materialColors_fake = new List<Material>();
    [SerializeField] List<Material> materialColors_sleep = new List<Material>();
    [SerializeField] GameObject blockerImage = default;
    [SerializeField] GameBoardMechanics gameboard = default;
    [SerializeField] GameObject smoke = default;
    [SerializeField] GameObject cube = default;
    [SerializeField] Material eggshell = default;
    [SerializeField] List<Vector3> zposition = new List<Vector3>();
    [SerializeField] List<Quaternion> zrotation = new List<Quaternion>();
    [SerializeField] GameObject sleepZzz = default;
    public float zDelayTimeMin;
    public float zDelayTimeMax;
    public float zzzTime;
    private Coroutine zCoroutine = null;
    public GameObject floatingGRP = default;
    public GameObject specialEffects1 = default;
    public GameObject specialEffects2 = default;
    public float shakeTime = 0f;
    public float shakeMag = 0f;
    public AnimationCurve ease;
    private Vector3 squareScale;
    private Vector3 floatingGRPScale;
    private Quaternion squareRotation;
    [SerializeField] GameObject middleSquare = default;
    [SerializeField] EyePickingMechanics eyeModeMechanics = default;
    public bool eyeMode = false;
    public bool eyePicker = false;
    public int eyePickerCountdown = 0;
    [SerializeField] RaycastMouse ray = default;
    [SerializeField] SwitchButton switchButton = default;
    private Vector3 squarePosition;
    public List<GameObject> arrows = new List<GameObject>();
    private Coroutine dimCo;
    public float dimmingDuration = 0.5f;
    [SerializeField] SpriteRenderer sleepingDim = default;
    public Color sleepColor;
    [SerializeField] Sprite dimMask = default;
    [SerializeField] Sprite dimMaskCutOut = default;
    [SerializeField] GameObject dimMaskSquareCut = default;
    public float luckyCoinPercentage = 0.25f;

    [Header("Swap Animation")]
    [SerializeField] private Transform route;
    private float tParam = 0f;
    private Vector2 routePos;
    public AnimationCurve swapEase;

    [Header("Lucky Coin")]
    [SerializeField] LuckyCoinReveal coin = default;
    public bool luckyCoin = false;
    public Color luckyCoin_yes;
    public Color luckyCoin_no;
    [SerializeField] float luckyCoinColorChangeDuration = 0.5f;
    [SerializeField] TextMeshPro luckyPointsText = default;
    [SerializeField] SpriteRenderer luckyPoints_bg = default;
    [SerializeField] SpriteRenderer luckyPoints_bgSmall = default;
    [SerializeField] GameObject luckyPoints_go = default;
    public List<Color> luckyTextColors = new List<Color>();
    public int luckyCoinMinMultiplier = 3;
    public int luckyCoinMaxMultiplier = 10;
    public AnimationCurve luckyPointsEase;
    public Coroutine luckyMiddleChangeCo = null;
    public Sprite miniSquare = default;
    public Sprite star = default;

    private Vector3 solid2_down;
    private Vector3 solid2_up;
    private Vector3 solid3_down;
    private Vector3 solid3_up;
    private Vector3 square_down;

    private MeshRenderer cube_mesh;
    private SpriteRenderer middleSquare_sprite;
    private FloatingSquare floatingGRP_floatingSquare;
    private CollectionColor_Sprite squareSprite_collection;

    private int number4Effects = 0;
    [SerializeField] List<GameObject> scoringEffect1 = new List<GameObject>();


    private void Awake()
    {
        floatingGRP_floatingSquare = floatingGRP.GetComponent<FloatingSquare>();
        middleSquare_sprite = middleSquare.GetComponent<SpriteRenderer>();
        cube_mesh = cube.GetComponent<MeshRenderer>();
        squareSprite_collection = squareSprite.GetComponent<CollectionColor_Sprite>();
        floatingGRPScale = floatingGRP.transform.localScale;
        squareScale = gameObject.transform.localScale;
        squareRotation = gameObject.transform.localRotation;
        squarePosition = gameObject.transform.localPosition;
    }

    private void Start() {
        GetSolidsPositions();
    }

    private void GetSolidsPositions() {
        solid2_down = new Vector3(solids[2].transform.localPosition.x, solids[2].transform.localPosition.y + .157f, solids[2].transform.localPosition.z);
        solid2_up = solids[2].transform.localPosition;
        solid3_down = new Vector3(solids[3].transform.localPosition.x, solids[3].transform.localPosition.y + .157f, solids[3].transform.localPosition.z);
        solid3_up = solids[3].transform.localPosition;
        square_down = new Vector3(squarePosition.x, squarePosition.y - .05f, squarePosition.z);
    }

    public void PrintPosition()
    {
        Debug.Log(gameObject.name + ": " + gameObject.transform.position.x + ", " + gameObject.transform.position.y);
    }

    public void LuckyCoinFound(int nextNumber)
    {
        float luckyCoinDraw = UnityEngine.Random.Range(0f, 1f);
        //Debug.Log("random "+luckyCoinDraw);
        //Debug.Log("drawl "+luckyCoinPercentage);
        if (luckyCoinDraw <= luckyCoinPercentage) {
            coin.LuckyCoinRevealAnimation();
        }
        else {
            int randomPointsAdded = UnityEngine.Random.Range((nextNumber * luckyCoinMinMultiplier), (nextNumber*luckyCoinMaxMultiplier)+1);
            PlaySpecialEffects();
            PlaySpecialEffects();
            gameboard.AddLuckyPoints(randomPointsAdded);
            LuckyPointsPopUp(randomPointsAdded,nextNumber);
        }
    }

    public void LuckyPointsPopUp(int number,int nextNumber) {
        luckyPointsText.text = "+" + number.ToString();
        SetLuckyTextColor(nextNumber);
        StartCoroutine(LuckyPointsText());
        StartCoroutine(LuckyPointsMove());
    }

    private void SetLuckyTextColor(int squareNumber) {
        luckyPointsText.color = luckyTextColors[squareNumber-1];
    }

    IEnumerator LuckyPointsMove() {
        Vector3 startPos = new Vector3(luckyPoints_go.transform.position.x, luckyPoints_go.transform.position.y, luckyPoints_go.transform.position.z);
        Vector3 endPos = new Vector3(luckyPoints_go.transform.position.x, luckyPoints_go.transform.position.y+1.5f, luckyPoints_go.transform.position.z);

        for (float t = 0; t < .75f; t += Time.deltaTime) {
            luckyPoints_go.transform.position = Vector3.Lerp(startPos, endPos, luckyPointsEase.Evaluate(t / .75f));
            yield return null;
        }
        yield return new WaitForSeconds(.5f);
        luckyPoints_go.transform.position = startPos;
    }

    IEnumerator LuckyPointsText() {
        Color invisibleColor = new Color(luckyPointsText.color.r, luckyPointsText.color.g, luckyPointsText.color.b, 0f);
        Color visibleColor = new Color(luckyPointsText.color.r, luckyPointsText.color.g, luckyPointsText.color.b, 1f);
        luckyPointsText.color = invisibleColor;
        luckyPointsText.gameObject.SetActive(true);
        for (float t = 0; t < 0.25f; t += Time.deltaTime) {
            luckyPointsText.color = Color.Lerp(invisibleColor, visibleColor, t / 0.25f);
            yield return null;
        }
        luckyPointsText.color = visibleColor;
        yield return new WaitForSeconds(0.75f);

        for (float t = 0; t < 0.25f; t += Time.deltaTime) {
            luckyPointsText.color = Color.Lerp(visibleColor, invisibleColor, t / 0.25f);
            yield return null;
        }
        luckyPointsText.color = invisibleColor;
        luckyPointsText.gameObject.SetActive(false);
    }

    public void SetLuckyColor() {
        if (luckyCoin) {
            middleSquare_sprite.sprite = star;
            middleSquare_sprite.color = new Color(1f, 1f, 1f, 1f);

            Hashtable hash = new Hashtable();
            hash.Add("amount", new Vector3(.75f, .75f, .75f));
            hash.Add("time", 1f);
            iTween.PunchScale(middleSquare, hash);

            SoundManager.SM.PlayOneShotSound("Bonus");
        }
        else {
            middleSquare_sprite.sprite = miniSquare;
            middleSquare_sprite.color = new Color(214f / 255f, 227f / 255f, 235f / 255f, 1f);
        }
    }

    public void SetSquareDisplay() {
        NumberDisplay();
        SetBlockerDisplay();
        EyeballDisplay();
    }

    public void SilentSquareDisplay() {
        SilentNumberDisplay();
        SilentBlockerDisplay();
        EyeballDisplay();
    }

    private void SilentBlockerDisplay()
    {
        if (number == 5)
        {
            blocker = true;
            completed = true;
            blockerImage.SetActive(true);
            floatingGRP.transform.position += new Vector3(0f,500f,0f);
        }
    }

    public void PressDown() {
        gameObject.transform.localPosition = square_down;
        solids[2].transform.localPosition = solid2_down;
        solids[3].transform.localPosition = solid3_down;
    }

    public void PressRelease() {
        gameObject.transform.localPosition = squarePosition;
        solids[2].transform.localPosition = solid2_up;
        solids[3].transform.localPosition = solid3_up;

    }

    private void SetBlockerDisplay()
    {
        if (number == 5)
        {
            blocker = true;
            completed = true;
            blockerImage.SetActive(true);
            Instantiate(smoke, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            floatingGRP_floatingSquare.QuickBurst();
            SoundManager.SM.PlayOneShotSound("poof");
        }
    }

    public void ConnectionDisplay() {
        SolidConnectionDisplay();
    }

    private void SolidConnectionDisplay() {
        connection_group.SetActive(true);
        for (int i = 0; i < adjescentConnections.Count; i++) {
            if (adjescentConnections[i] == true) {
                solids[i].SetActive(true);
            }
            else {
                solids[i].SetActive(false);
            }
        }
    }

    private void EyeballDisplay() {
        if (number < 5) {

            int eyeShutCounter = 0;
            for (int i = 0; i < adjescentConnections.Count; i++) {
                if (adjescentConnections[i] == true) {
                    eyeShutCounter++;
                }
            }

            faces[number - 1].ShutThisManyEyes(eyeShutCounter);

        }
        gameboard.UpdateSquareConnections();
        //ConnectionDisplay();
    }

    private void SilentNumberDisplay() {
        SetSquareColor();
        SetSquareFace();
    }

    private void NumberDisplay() {
        SetSquareColor();
        SetSquareFace();
        PopSFX();
        Pop();
    }

    private void SetSquareFace() {
        if (number < 5) {
            faces[number - 1].gameObject.SetActive(true);
            faces[number - 1].StartFacialAnimation();
        }
    }

    private void SetSquareColor()
    {
        squareSprite.color = numberColors[number - 1];

        //Debug.LogWarning(numberColors[number - 1]);

        cube_mesh.material = materialColors[number - 1];
        LittleSquareDisplay();
    }

    public void SwitchToFakeMaterials()
    {
        if (number > 0 && number <5)
        {
            //Debug.Log(number);
            if (completed)
            {
                cube_mesh.material = materialColors_sleep[number - 1];
                sleepingDim.color = new Color(sleepColor.r, sleepColor.g, sleepColor.b, 0f);
                dimMaskSquareCut.SetActive(true);
            }
            else
            {
                cube_mesh.material = materialColors_fake[number - 1];
            }
        }
        else
        {
            cube_mesh.material = materialColors_fake[4];
        }

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

    private void PopSFX() {
        switch (number) {
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
        gameObject.transform.localPosition = new Vector3(squarePosition.x, squarePosition.y, squarePosition.z-.2f);
        gameboard.AllSquaresDown(gameObject);
        floatingGRP.transform.localScale = floatingGRPScale;

        squareSprite.sortingOrder = 1;
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 1f);
        hash.Add("oncomplete", "PopDone");
        hash.Add("oncompletetarget", gameObject);
        iTween.PunchScale(floatingGRP, hash);
    }

    private void PopDone() {
        //gameObject.transform.localScale = squareScale;
        floatingGRP.transform.localScale = floatingGRPScale;
        if (!eyeMode && switchButton.activated==false)
        {
            squareSprite.sortingOrder = 0;
        }
        SquareDown();
    }

    private void PopAway() {
        
        floatingGRP.transform.localScale = floatingGRPScale;
        squareSprite.sortingOrder = 1;
        Hashtable hash = new Hashtable();
        hash.Add("amount", new Vector3(1f, 1f, 0f));
        hash.Add("time", 1f);
        hash.Add("oncomplete", "PopAwayDone");
        hash.Add("oncompletetarget", gameObject);
        iTween.PunchScale(floatingGRP, hash);
    }

    private void PopAwayDone() {
        squareSprite.sortingOrder = 0;
        //gameObject.transform.localScale = squareScale;
        floatingGRP.transform.localScale = floatingGRPScale;
    }


    public void RecalculateAdjescentSquares() {
        for (int i = 0; i < adjescentSquares.Count; i++) {
            if (adjescentSquares[i] != null) {
                if (adjescentSquares[i].completed == false && adjescentSquares[i].number > 0) {
                    adjescentSquares[i].CalculateConnections();
                }
            }
        }
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

    public void CalculateConnections() {
        if (blocker == false) {
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

    private SquareMechanics_Gameboard GetEyePickerSquare()
    {
        SquareMechanics_Gameboard eyePickerSquare = null;
    
        for (int i = 0; i < gameboard.gameBoardSquaresMechanics.Count; i++)
        {
            if (gameboard.gameBoardSquaresMechanics[i].eyePicker)
            {
                eyePickerSquare = gameboard.gameBoardSquaresMechanics[i];
            }
        }

        return eyePickerSquare;
    }

    private int GetAdjescentConnectionNumberOfEyePicker(SquareMechanics_Gameboard matchingSquare)
    {
        int num = 0;
        for (int i = 0; i < adjescentSquares.Count; i++)
        {
            //Debug.LogWarning(adjescentSquares[i]);
            //Debug.LogWarning(matchingSquare);

            if (adjescentSquares[i]==matchingSquare)
            {
                //Debug.LogWarning("match "+ i);
                num = i;
            }
        }
        return num;
    }

    public void EyeModeUpdate(){
        SquareMechanics_Gameboard eyePicker = GetEyePickerSquare();
        int adjescentIndex = GetAdjescentConnectionNumberOfEyePicker(eyePicker);
        adjescentConnections[adjescentIndex] = true;
        eyePicker.adjescentConnections[GetOppositeRandomOrderNumber(adjescentIndex)] = true;
        eyeMode = false;
        eyePicker.eyePickerCountdown--;

        eyeModeMechanics.UnHilightSquare(gameObject);
        eyeModeMechanics.UnArrowSquare(gameObject);

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

    public void ResetSquare_OnClick() {
        RemoveConnectionsOnAdjescentSquareInfo();
        ZeroOutSquareInfo();
    }



    private void RemoveConnectionsOnAdjescentSquareInfo() {

        for (int i = 0; i < adjescentConnections.Count; i++) {

            if (adjescentConnections[i] == true) {
                if (i == 0) {
                    adjescentSquares[i].adjescentConnections[1] = false;
                }
                else if (i == 1) {
                    adjescentSquares[i].adjescentConnections[0] = false;
                }
                else if (i == 2) {
                    adjescentSquares[i].adjescentConnections[3] = false;
                }
                if (i == 3) {
                    adjescentSquares[i].adjescentConnections[2] = false;
                }
                
                adjescentSquares[i].CheckCompleted();
            }

        }
    }

    public void ResetSquare_BlockerClear() {
        PopAway();
        ZeroOutSquareInfo();
    }

    public void ZeroOutSquareInfo() {
        connection_group.SetActive(false);
        number = 0;
        adjescentConnections = new List<bool> { false, false, false, false };
        completed = false;
        TurnOffFaces();
        squareSprite_collection.GetColor();
        cube_mesh.material = eggshell;
        blocker = false;
        blockerImage.SetActive(false);
        SetSquarePosition();
        LittleSquareDisplay();
    }

    public void ZeroOutSquareInfo_BeforeCompletion()
    {
        number4Effects = number;
        number = 0;
        adjescentConnections = new List<bool> { false, false, false, false };
        //Debug.LogWarning(squareSprite.transform.parent.parent.gameObject.name + " turned completed false" );
        completed = false;
        blocker = false;
        blockerImage.SetActive(false);
        SetSquarePosition();
    }

    public void ZeroOutSquareInfo_AfterCompletion()
    {
        SetSquarePosition();
        //StartCoroutine(ChangeColorDelay());
        connection_group.SetActive(false);
        TurnOffFaces();
        squareSprite_collection.GetColor();
        cube_mesh.material = eggshell;
        LittleSquareDisplay();
    }

    IEnumerator ChangeColorDelay() {
        yield return new WaitForSeconds(0.05f);
        connection_group.SetActive(false);
        TurnOffFaces();
        squareSprite_collection.GetColor();
        cube_mesh.material = eggshell;
        LittleSquareDisplay();
    }

    private void SetSquarePosition() {
        floatingGRP_floatingSquare.StopFloat();
        gameObject.transform.localRotation = squareRotation;
        gameObject.transform.localPosition = squarePosition;
    }

    private void TurnOffFaces() {
        for(int i = 0; i < faces.Count; i++) {
            if (faces[i].gameObject.activeSelf == true) {
                faces[i].StopFacialAnimation();
                faces[i].ResetEyes();
                faces[i].gameObject.SetActive(false);
            }
        }
    }

    private int GetOppositeRandomOrderNumber(int number) {
        int oppositeNumber = 0;
        switch (number) {
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

    private List<int> GetRandomOrder() {
        List<int> order = new List<int>() { 0, 1, 2, 3 };
        for (int i = 0; i < order.Count; i++) {
            int tempNumber = order[0];
            int randomIndex = UnityEngine.Random.Range(0, 4);
            order[0] = order[randomIndex];
            order[randomIndex] = tempNumber;
        }
        return order;
    }

    private List<bool> GetAdjescentSquaresEligible() {
        List<bool> adjescentSquaresEligible = new List<bool> { false, false, false, false };
        adjescentSquaresEligible[0] = GetSquareEligibility(adjescentSquares[0],"Top");
        adjescentSquaresEligible[1] = GetSquareEligibility(adjescentSquares[1], "Bottom");
        adjescentSquaresEligible[2] = GetSquareEligibility(adjescentSquares[2], "Left");
        adjescentSquaresEligible[3] = GetSquareEligibility(adjescentSquares[3], "Right");
        return adjescentSquaresEligible;
    }

    private bool GetSquareEligibility(SquareMechanics_Gameboard square,string side) {
        bool eligible = false;
        if (square != null) {
            if (square.completed == false && square.number > 0) {
                eligible = true;
                //Debug.Log(square.gameObject.name + " (" + side + ")" + " eligible");
            }
            //else {
                //Debug.Log(square.gameObject.name + " (" + side + ")" + " not eligible");
            //}
        }
        return eligible;
    }


    private int GetCurrentTotalConnections() {
        int connections = 0;

        for (int i = 0; i < adjescentConnections.Count; i++) {
            if (adjescentConnections[i]) {
                connections++;
            }
        }
        return connections;
    }

    public void CheckCompleted() {
        //Debug.LogWarning(squareSprite.transform.parent.parent.gameObject.name + " check completed");
        int connections = GetCurrentTotalConnections();
        
        if (connections == number)
        {
            //Debug.LogWarning("test1");
            completed = true;
            CheckDimIfCompleted();
            //Debug.LogWarning(squareSprite.transform.parent.parent.gameObject.name + " " + completed);
            faces[number-1].StopFacialAnimation();
            EyeballDisplay();

            if (zCoroutine == null)
            {
                zCoroutine = StartCoroutine(DelayThenZzz());
            }
            else
            {
                StopCoroutine(zCoroutine);
                zCoroutine = StartCoroutine(DelayThenZzz());
            }

            gameboard.CheckForCompleteLink(gameObject);
        }
        else {
            //Debug.LogWarning("test2");
            completed = false;
            CheckDimIfCompleted();
            //Debug.LogWarning(squareSprite.transform.parent.parent.gameObject.name + " " + completed);
            faces[number-1].StartFacialAnimation();
            EyeballDisplay();
        }
        
    }

    public void CheckDimIfCompleted()
    {
        SetDimSprite();
        if (completed)
        {

            if (dimCo == null )
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
            if (gameboard.playSquareClearAnimation==false)
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
        dimMaskSquareCut.SetActive(false);
        if (number > 0 && number < 5)
        {
            if(sleepingDim.color != sleepColor)
            {
                Color alphaColor = new Color(sleepColor.r, sleepColor.g, sleepColor.b,0f);
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
                adjescentSquares[i].faces[adjescentSquares[i].number - 1].ShutThisManyEyes(eyeShutCounter-1);
            }
        }
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

    public void UnDimSquare()
    {
        dimMaskSquareCut.SetActive(false);
        sleepingDim.color = new Color(sleepColor.r, sleepColor.g, sleepColor.b, 0f);
    }

    public void ResetSquare_OnCompletion_Before()
    {
        ZeroOutSquareInfo_BeforeCompletion();
    }

    public void ResetSquare_OnCompletion_After()
    {
        StopZzz();
        //StartCoroutine(PopAwayDelay());
        PopAway();
        PlayScoringEffects();
        ZeroOutSquareInfo_AfterCompletion();
    }
    IEnumerator PopAwayDelay() {
        yield return new WaitForSeconds(0.05f);
        floatingGRP.SetActive(false);
        yield return new WaitForSeconds(0.75f);

        floatingGRP.SetActive(true);
        PopAway();
    }

    private void PlaySpecialEffects()
    {
        Instantiate(specialEffects1, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        Instantiate(specialEffects2, gameObject.transform.position,Quaternion.identity, gameObject.transform);
    }

    private void PlayScoringEffects() {
        GameObject effect1 = (GameObject)Instantiate(scoringEffect1[number4Effects - 1], gameObject.transform.position + new Vector3(0f, 0f, -1f), Quaternion.identity, gameObject.transform);
        effect1.transform.Rotate(new Vector3(210f, 0f, 0f));
    }

    public void StopZzz()
    {
        if (zCoroutine != null)
        {
            StopCoroutine(zCoroutine);
        }

        //Debug.Log(gameObject.name + "TurnOffZzzs");
        sleepZzz.SetActive(false);
    }

    private IEnumerator DelayThenZzz()
    {
        sleepZzz.transform.localPosition = zposition[number - 1];
        sleepZzz.transform.localRotation = zrotation[number-1];

        float delayTime = UnityEngine.Random.Range(zDelayTimeMin, zDelayTimeMax);
        yield return new WaitForSeconds(delayTime);
        sleepZzz.SetActive(true);
        yield return new WaitForSeconds(zzzTime);
        yield return new WaitForSeconds(.5f);
        sleepZzz.SetActive(false);
        //Debug.Log("delayTime: " + delayTime);
        //Debug.Log("holdTime: " + holdTime);
    }

    public void SetAdjescentSquares() {
        adjescentSquares[0] = SetTopSquare();
        adjescentSquares[1] = SetBottomSquare();
        adjescentSquares[2] = SetLeftSquare();
        adjescentSquares[3] = SetRightSquare();
    }

    private SquareMechanics_Gameboard SetTopSquare() {
        int topSquare_gamePositionY = gamePositionY + 1;
        int height = gameboard.gameBoardHeight;
        bool edgeCase = false;
        if (topSquare_gamePositionY >= height) {
            edgeCase = true;
        }
        if (!edgeCase) {
            int index = gamePositionIndex + 1;
            return gameboard.gameBoardSquaresMechanics[index];
        }
        else {
            //Debug.Log(gameObject.name + " has no top");
            return null;
        }
    }

    private SquareMechanics_Gameboard SetBottomSquare() {
        int botSquare_gamePositionY = gamePositionY - 1;
        bool edgeCase = false;
        if (botSquare_gamePositionY < 0) {
            edgeCase = true;
        }
        if (!edgeCase) {
            int index = gamePositionIndex - 1;
            return gameboard.gameBoardSquaresMechanics[index];
        }
        else {
            //Debug.Log(gameObject.name + " has no bottom");
            return null;
        }
    }

    private SquareMechanics_Gameboard SetLeftSquare() {
        int leftSquare_gamePositionX = gamePositionX - 1;
        bool edgeCase = false;
        if (leftSquare_gamePositionX < 0) {
            edgeCase = true;
        }
        if (!edgeCase) {
            int height = gameboard.gameBoardHeight;
            int index = gamePositionIndex - height;
            return gameboard.gameBoardSquaresMechanics[index];
        }
        else {
            //Debug.Log(gameObject.name + " has no left");
            return null;
        }
    }

    private SquareMechanics_Gameboard SetRightSquare() {
        int width = gameboard.gameBoardWidth;
        int rightSquare_gamePositionX = gamePositionX + 1;
        bool edgeCase = false;
        if (rightSquare_gamePositionX >= width) {
            edgeCase = true;
        }
        if (!edgeCase) {
            int height = gameboard.gameBoardHeight;
            int index = gamePositionIndex + height;
            return gameboard.gameBoardSquaresMechanics[index];
        }
        else {
            //Debug.Log(gameObject.name + " has no right");
            return null;
        }
    }

    public void ShakeSquare()
    {
        StartCoroutine(Shake(shakeTime,shakeMag));
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

    public void MoveAlongRoute(float timeDuration)
    {
        StartCoroutine(MoveSquareOnRoute(timeDuration));
    }

    IEnumerator MoveSquareOnRoute(float timeDuration)
    {
        Vector2 p0 = route.GetChild(0).position;
        Vector2 p1 = route.GetChild(1).position;
        Vector2 p2 = route.GetChild(2).position;
        Vector2 p3 = route.GetChild(3).position;


        for (float t = 0f;  t < timeDuration; t+=Time.deltaTime)
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

}
